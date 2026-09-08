using System.Globalization;
using Microsoft.Data.Sqlite;

namespace Tintelo.iOS.Utils;

internal static class DatabaseMigrations
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
			CREATE TABLE Entries (
				Id TEXT NOT NULL PRIMARY KEY,
				JournalDate TEXT NOT NULL UNIQUE,
				Mood INTEGER NOT NULL CHECK (Mood BETWEEN -3 AND 3),
				Note TEXT NOT NULL DEFAULT '',
				CreatedUtc TEXT NOT NULL,
				UpdatedUtc TEXT NOT NULL
			);

			CREATE TABLE Categories (
				Id TEXT NOT NULL PRIMARY KEY,
				Name TEXT NOT NULL CHECK (length(trim(Name)) > 0),
				Emoji TEXT NOT NULL DEFAULT '',
				Color TEXT NOT NULL,
				SortOrder INTEGER NOT NULL,
				IsArchived INTEGER NOT NULL DEFAULT 0 CHECK (IsArchived IN (0, 1))
			);

			CREATE TABLE EntryCategories (
				EntryId TEXT NOT NULL REFERENCES Entries(Id) ON DELETE CASCADE,
				CategoryId TEXT NOT NULL REFERENCES Categories(Id) ON DELETE RESTRICT,
				PRIMARY KEY (EntryId, CategoryId)
			);

			CREATE INDEX IX_EntryCategories_CategoryId_EntryId
			ON EntryCategories (CategoryId, EntryId);

			CREATE TABLE LibraryMetadata (
				Id INTEGER NOT NULL PRIMARY KEY CHECK (Id = 1),
				LibraryId TEXT NOT NULL UNIQUE,
				Revision INTEGER NOT NULL DEFAULT 0 CHECK (Revision >= 0)
			);

			INSERT INTO LibraryMetadata (Id, LibraryId, Revision)
			VALUES (1, $libraryId, 0);

			PRAGMA user_version = 1;
			""";
		command.Parameters.AddWithValue("$libraryId", Guid.NewGuid().ToString("D"));
		command.ExecuteNonQuery();
	}

	
	internal static void Apply(
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
