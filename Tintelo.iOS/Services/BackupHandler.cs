using Microsoft.Data.Sqlite;
using Tintelo.iOS.Models;

namespace Tintelo.iOS.Services;

public sealed class BackupHandler(
	Database database)
{
	public Task<Library> ReadLibraryAsync(
		string snapshotPath,
		CancellationToken cancellationToken = default) =>
		database.ReadSnapshotAsync(snapshotPath, snapshot =>
		{
			using SqliteCommand command = snapshot.CreateCommand();
			command.CommandText = "SELECT Id, Revision FROM Library;";
			
			using SqliteDataReader reader = command.ExecuteReader();
			if (!reader.Read())
				throw new InvalidDataException("The snapshot is missing its library metadata.");

			Library library = new(reader.GetGuid(0), reader.GetInt64(1));
			if (reader.Read())
				throw new InvalidDataException("The snapshot contains more than one library.");

			return library;
		}, cancellationToken);
}
