using Microsoft.Data.Sqlite;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public sealed class EntryRepository(
	Database database)
{
	static Entry? ReadEntry(
		SqliteConnection connection,
		SqliteTransaction transaction,
		string statement,
		string value)
	{
		using SqliteCommand command = connection.CreateCommand(transaction, statement, ("$value", value));
		using SqliteDataReader reader = command.ExecuteReader();
		if (!reader.Read())
			return null;

		Guid id = reader.GetGuid(0);
		DateOnly date = SqliteValues.ToDate(reader.GetString(1));
		Mood mood = (Mood)reader.GetInt32(2);
		string? note = reader.IsDBNull(3) ? null : reader.GetString(3);
		DateTime createdUtc = SqliteValues.ToUtc(reader.GetString(4));
		DateTime updatedUtc = SqliteValues.ToUtc(reader.GetString(5));

		reader.Close();

		return new Entry(id, date, mood, note, ReadCategories(connection, transaction, id), createdUtc, updatedUtc);
	}

	static IReadOnlyList<Category> ReadCategories(
		SqliteConnection connection,
		SqliteTransaction transaction,
		Guid entryId)
	{
		using SqliteCommand command = connection.CreateCommand(
			transaction,
			"""
			SELECT c.Id, c.Name, c.Emoji, c.Color, c.IsArchived
			FROM EntryCategory ec
			JOIN Category c ON c.Id = ec.CategoryId
			WHERE ec.EntryId = $entryId;
			""",
			("$entryId", entryId.ToString("D")));

		using SqliteDataReader reader = command.ExecuteReader();

		List<Category> categories = [];
		while (reader.Read())
			categories.Add(CategoryRepository.ReadCategory(reader));

		return categories;
	}

	static IReadOnlyList<EntrySummary> ReadSummaries(
		SqliteConnection connection,
		SqliteTransaction transaction,
		string statement,
		params (string Name, object? Value)[] parameters)
	{
		using SqliteCommand command = connection.CreateCommand(transaction, statement, parameters);
		using SqliteDataReader reader = command.ExecuteReader();

		List<EntrySummary> summaries = [];
		while (reader.Read())
		{
			summaries.Add(new EntrySummary(
				SqliteValues.ToDate(reader.GetString(0)),
				(Mood)reader.GetInt32(1),
				reader.GetBoolean(2)));
		}

		return summaries;
	}
	
	static void ReplaceCategories(
		SqliteConnection connection,
		SqliteTransaction transaction,
		Guid entryId,
		IReadOnlyList<Guid> categoryIds)
	{
		using (SqliteCommand delete = connection.CreateCommand(
			transaction,
			"DELETE FROM EntryCategory WHERE EntryId = $entryId;",
			("$entryId", entryId.ToString("D"))))
		{
			delete.ExecuteNonQuery();
		}

		using SqliteCommand insert = connection.CreateCommand(
			transaction,
			"INSERT INTO EntryCategory (EntryId, CategoryId) VALUES ($entryId, $categoryId);",
			("$entryId", entryId.ToString("D")));

		SqliteParameter categoryParameter = insert.Parameters.Add("$categoryId", SqliteType.Text);

		foreach (Guid categoryId in categoryIds.Distinct())
		{
			categoryParameter.Value = categoryId.ToString("D");
			insert.ExecuteNonQuery();
		}
	}


	public Task<Entry?> GetAsync(
		Guid id,
		CancellationToken cancellationToken = default) =>
		database.ReadAsync((connection, transaction) =>
			ReadEntry(
				connection,
				transaction,
				"SELECT Id, Date, Mood, Note, CreatedUtc, UpdatedUtc FROM Entry WHERE Id = $value;",
				id.ToString("D")),
			cancellationToken);
	public Task<Entry?> GetByDateAsync(
		DateOnly date,
		CancellationToken cancellationToken = default) =>
		database.ReadAsync((connection, transaction) =>
			ReadEntry(
				connection,
				transaction,
				"SELECT Id, Date, Mood, Note, CreatedUtc, UpdatedUtc FROM Entry WHERE Date = $value;",
				SqliteValues.ToText(date)),
			cancellationToken);

	public Task<IReadOnlyList<EntrySummary>> GetSummariesAsync(
		CancellationToken cancellationToken = default) =>
		database.ReadAsync(
			(connection, transaction) => ReadSummaries(
				connection,
				transaction,
				"""
				SELECT Date, Mood, Note IS NOT NULL AS HasNote
				FROM Entry
				ORDER BY Date;
				"""),
			cancellationToken);
	public Task<IReadOnlyList<EntrySummary>> GetSummariesAsync(
		DateOnly from,
		DateOnly to,
		CancellationToken cancellationToken = default) =>
		database.ReadAsync(
			(connection, transaction) => ReadSummaries(
				connection,
				transaction,
				"""
				SELECT Date, Mood, Note IS NOT NULL AS HasNote
				FROM Entry
				WHERE Date BETWEEN $from AND $to
				ORDER BY Date;
				""",
				("$from", SqliteValues.ToText(from)),
				("$to", SqliteValues.ToText(to))),
			cancellationToken);

	public Task<Entry> CreateAsync(
		DateOnly date,
		Mood mood,
		string? note,
		IReadOnlyList<Guid> categoryIds,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			Guid id = Guid.CreateVersion7();
			string? normalizedNote = string.IsNullOrWhiteSpace(note) ? null : note;
			DateTime now = DateTime.UtcNow;

			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				INSERT INTO Entry (Id, Date, Mood, Note, CreatedUtc, UpdatedUtc)
				VALUES ($id, $date, $mood, $note, $now, $now);
				""",
				("$id", id.ToString("D")),
				("$date", SqliteValues.ToText(date)),
				("$mood", (int)mood),
				("$note", normalizedNote),
				("$now", SqliteValues.ToUtcText(now)));

			command.ExecuteNonQuery();
			ReplaceCategories(connection, transaction, id, categoryIds);

			return new Entry(id, date, mood, normalizedNote, ReadCategories(connection, transaction, id), now, now);
		}, cancellationToken);

	public Task<bool> UpdateAsync(
		Guid id,
		DateOnly date,
		Mood mood,
		string? note,
		IReadOnlyList<Guid> categoryIds,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				UPDATE Entry
				SET Date = $date, Mood = $mood, Note = $note, UpdatedUtc = $now
				WHERE Id = $id;
				""",
				("$id", id.ToString("D")),
				("$date", SqliteValues.ToText(date)),
				("$mood", (int)mood),
				("$note", string.IsNullOrWhiteSpace(note) ? null : note),
				("$now", SqliteValues.ToUtcText(DateTime.UtcNow)));

			if (command.ExecuteNonQuery() != 1)
				return false;

			ReplaceCategories(connection, transaction, id, categoryIds);
			return true;
		}, cancellationToken);

	public Task<bool> DeleteAsync(
		Guid id,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"DELETE FROM Entry WHERE Id = $id;",
				("$id", id.ToString("D")));

			return command.ExecuteNonQuery() == 1;
		}, cancellationToken);
}
