using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public sealed class DatabaseService(
	ILogger<DatabaseService> logger) : IAsyncDisposable
{
	readonly SemaphoreSlim gate = new(1, 1);
	
	SqliteConnection? connection;
	bool disposed;

	
	static string ConnectionString(
		string path,
		SqliteOpenMode mode = SqliteOpenMode.ReadWriteCreate) =>
		new SqliteConnectionStringBuilder
		{
			DataSource = path,
			Mode = mode,
			ForeignKeys = true,
			Pooling = false,
			DefaultTimeout = 5
		}.ToString();

	
	SqliteConnection GetConnection(
		CancellationToken cancellationToken)
	{
		ObjectDisposedException.ThrowIf(disposed, this);
		if (connection is not null)
			return connection;

		Directory.CreateDirectory(Path.GetDirectoryName(Paths.Database)!);
		SqliteConnection opened = new(ConnectionString(Paths.Database));
		try
		{
			opened.Open();
			
			using SqliteCommand command = opened.CreateCommand();
			command.CommandText = "PRAGMA journal_mode = WAL;";
			command.ExecuteNonQuery();
			
			DatabaseMigrations.Apply(opened, cancellationToken);
			
			logger.LogInformation("Opened database at '{DatabasePath}'.", Paths.Database);
			return connection = opened;
		}
		catch
		{
			opened.Dispose();
			throw;
		}
	}

	async Task<TResult> ExecuteAsync<TResult>(
		Func<TResult> operation,
		CancellationToken cancellationToken)
	{
		await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			return await Task.Run(operation, cancellationToken).ConfigureAwait(false);
		}
		finally
		{
			gate.Release();
		}
	}

	Task<TResult> TransactionAsync<TResult>(
		Func<SqliteConnection, SqliteTransaction, TResult> operation,
		bool write,
		CancellationToken cancellationToken) =>
		ExecuteAsync(() =>
		{
			SqliteConnection database = GetConnection(cancellationToken);
			
			using SqliteTransaction transaction = database.BeginTransaction(deferred: !write);
			cancellationToken.ThrowIfCancellationRequested();
			
			TResult result = operation(database, transaction);
			if (write)
			{
				using SqliteCommand command = database.CreateCommand();
				command.Transaction = transaction;
				command.CommandText = "UPDATE LibraryMetadata SET Revision = Revision + 1 WHERE Id = 1;";
				command.ExecuteNonQuery();
			}
			
			cancellationToken.ThrowIfCancellationRequested();
			transaction.Commit();
			
			return result;
		}, cancellationToken);

	
	public Task InitializeAsync(
		CancellationToken cancellationToken = default) =>
		ExecuteAsync(() => GetConnection(cancellationToken), cancellationToken);

	public ValueTask DisposeAsync() =>
		new(ExecuteAsync(() =>
		{
			disposed = true;
			connection?.Dispose();
			connection = null;
			return true;
		}, CancellationToken.None));

	
	public Task<TResult> ReadAsync<TResult>(
		Func<SqliteConnection, SqliteTransaction, TResult> read,
		CancellationToken cancellationToken = default) =>
		TransactionAsync(read, false, cancellationToken);

	public Task<TResult> WriteAsync<TResult>(
		Func<SqliteConnection, SqliteTransaction, TResult> write,
		CancellationToken cancellationToken = default) =>
		TransactionAsync(write, true, cancellationToken);

	public Task WriteAsync(
		Action<SqliteConnection, SqliteTransaction> write,
		CancellationToken cancellationToken = default) =>
		WriteAsync((database, transaction) =>
		{
			write(database, transaction);
			return true;
		}, cancellationToken);

	
	public Task<TResult> ReadSnapshotAsync<TResult>(
		string snapshotPath,
		Func<SqliteConnection, TResult> read,
		CancellationToken cancellationToken = default) =>
		ExecuteAsync(() =>
		{
			ObjectDisposedException.ThrowIf(disposed, this);
			
			using SqliteConnection snapshot = new(ConnectionString(Path.GetFullPath(snapshotPath), SqliteOpenMode.ReadOnly));
			snapshot.Open();
			
			cancellationToken.ThrowIfCancellationRequested();
			TResult result = read(snapshot);
			
			return result;
		}, cancellationToken);

	public Task CreateSnapshotAsync(
		string destinationPath,
		CancellationToken cancellationToken = default) =>
		ExecuteAsync(() =>
		{
			SqliteConnection database = GetConnection(cancellationToken);
			
			string destination = Path.GetFullPath(destinationPath);
			if (File.Exists(destination))
				throw new IOException($"The snapshot destination '{destination}' already exists.");

			Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
			string stagingPath = destination + $".{Guid.NewGuid():N}.tmp";
			try
			{
				using (SqliteConnection snapshot = new(ConnectionString(stagingPath)))
				{
					snapshot.Open();
					database.BackupDatabase(snapshot);
				}
				
				cancellationToken.ThrowIfCancellationRequested();
				File.Move(stagingPath, destination);
				
				return true;
			}
			finally
			{
				File.Delete(stagingPath);
			}
		}, cancellationToken);
}
