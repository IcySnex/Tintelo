using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public sealed class Database(
	ILogger<Database> logger) : IAsyncDisposable
{
	static class Migrations
	{
		const int CurrentVersion = 1;
	
	
		static int ReadVersion(
			SqliteConnection connection,
			SqliteTransaction transaction)
		{
			using SqliteCommand command = connection.CreateCommand();
			command.Transaction = transaction;
			command.CommandText = "PRAGMA user_version;";
			
			return Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture);
		}
	
		static void ApplyVersion1(
			SqliteConnection connection,
			SqliteTransaction transaction)
		{
			using SqliteCommand command = connection.CreateCommand();
			command.Transaction = transaction;
			command.CommandText =
				"""
				PRAGMA application_id = 0x54494E54;
	
				CREATE TABLE Entry (
					Id TEXT NOT NULL PRIMARY KEY,
					Date TEXT NOT NULL CHECK (date(Date) IS Date),
					Mood INTEGER NOT NULL CHECK (Mood BETWEEN -3 AND 3),
					Note TEXT,
					CreatedUtc TEXT NOT NULL,
					UpdatedUtc TEXT NOT NULL
				) STRICT;
	
				CREATE UNIQUE INDEX UX_Entry_Date
				ON Entry (Date);
	
				CREATE TABLE Category (
					Id TEXT NOT NULL PRIMARY KEY,
					Name TEXT NOT NULL CHECK (length(trim(Name)) > 0),
					Emoji TEXT,
					Color TEXT NOT NULL CHECK (
						length(Color) = 7
						AND Color GLOB '#[0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f]'
					),
					IsArchived INTEGER NOT NULL DEFAULT 0 CHECK (IsArchived IN (0, 1))
				) STRICT;
	
				CREATE TABLE EntryCategory (
					EntryId TEXT NOT NULL REFERENCES Entry(Id) ON DELETE CASCADE,
					CategoryId TEXT NOT NULL REFERENCES Category(Id) ON DELETE RESTRICT,
					PRIMARY KEY (EntryId, CategoryId)
				) STRICT, WITHOUT ROWID;
	
				CREATE INDEX IX_EntryCategory_CategoryId_EntryId
				ON EntryCategory (CategoryId, EntryId);
	
				CREATE TABLE Library (
					Id TEXT NOT NULL PRIMARY KEY,
					Revision INTEGER NOT NULL DEFAULT 0 CHECK (Revision >= 0)
				) STRICT;
	
				INSERT INTO Library (Id, Revision)
				VALUES ($libraryId, 0);
	
				PRAGMA user_version = 1;
				""";
			command.Parameters.AddWithValue("$libraryId", Guid.CreateVersion7().ToString("D"));
			command.ExecuteNonQuery();
		}
	
		
		public static void Apply(
			SqliteConnection connection,
			CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			
			using SqliteTransaction transaction = connection.BeginTransaction(false);
			
			int version = ReadVersion(connection, transaction);
			switch (version)
			{
				case < 0:
				case > CurrentVersion:
					throw new InvalidOperationException($"Database schema version {version} is not supported (current version: {CurrentVersion}).");
				
				case < 1:
					ApplyVersion1(connection, transaction);
					break;
			}
			
			cancellationToken.ThrowIfCancellationRequested();
			transaction.Commit();
		}
	}
	
	
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
			
			Migrations.Apply(opened, cancellationToken);
			
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
				command.CommandText = "UPDATE Library SET Revision = Revision + 1;";
				if (command.ExecuteNonQuery() != 1)
					throw new InvalidOperationException("The library row is missing or duplicated; the revision could not be updated.");
			}
			
			cancellationToken.ThrowIfCancellationRequested();
			transaction.Commit();
			
			return result;
		}, cancellationToken);

	
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
