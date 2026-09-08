using Microsoft.Data.Sqlite;
using Tintelo.iOS.Models;

namespace Tintelo.iOS.Services;

public sealed class BackupService(
	DatabaseService databaseService)
{
	public Task<LibraryMetadata> ReadMetadataAsync(
		string snapshotPath,
		CancellationToken cancellationToken = default) =>
		databaseService.ReadSnapshotAsync(snapshotPath, snapshot =>
		{
			using SqliteCommand command = snapshot.CreateCommand();
			command.CommandText = "SELECT LibraryId, Revision FROM LibraryMetadata WHERE Id = 1;";
			
			using SqliteDataReader reader = command.ExecuteReader();
			if (!reader.Read())
				throw new InvalidDataException("The snapshot is missing its library metadata.");

			return new LibraryMetadata(reader.GetGuid(0), reader.GetInt64(1));
		}, cancellationToken);
}
