using Microsoft.Data.Sqlite;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public sealed class CategoryRepository(
	Database database)
{
	const string SelectAll = "SELECT Id, Name, Emoji, Color, IsArchived FROM Category;";
	const string SelectActive = "SELECT Id, Name, Emoji, Color, IsArchived FROM Category WHERE IsArchived = 0;";


	internal static Category ReadCategory(
		SqliteDataReader reader) =>
		new(
			reader.GetGuid(0),
			reader.GetString(1),
			reader.IsDBNull(2) ? null : reader.GetString(2),
			reader.GetString(3),
			reader.GetBoolean(4));


	public Task<IReadOnlyList<Category>> GetAsync(
		bool includeArchived = false,
		CancellationToken cancellationToken = default) =>
		database.ReadAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				includeArchived ? SelectAll : SelectActive);

			using SqliteDataReader reader = command.ExecuteReader();

			List<Category> categories = [];
			while (reader.Read())
				categories.Add(ReadCategory(reader));

			return (IReadOnlyList<Category>)categories;
		}, cancellationToken);

	public Task<Category> CreateAsync(
		string name,
		string? emoji,
		string color,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			Guid id = Guid.CreateVersion7();
			string trimmedName = name.Trim();
			string? trimmedEmoji = string.IsNullOrWhiteSpace(emoji) ? null : emoji.Trim();
			string trimmedColor = color.Trim();

			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				INSERT INTO Category (Id, Name, Emoji, Color, IsArchived)
				VALUES ($id, $name, $emoji, $color, 0);
				""",
				("$id", id.ToString("D")),
				("$name", trimmedName),
				("$emoji", trimmedEmoji),
				("$color", trimmedColor));

			command.ExecuteNonQuery();

			return new Category(id, trimmedName, trimmedEmoji, trimmedColor, false);
		}, cancellationToken);

	public Task<bool> UpdateAsync(
		Guid id,
		string name,
		string? emoji,
		string color,
		bool isArchived,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				UPDATE Category
				SET Name = $name, Emoji = $emoji, Color = $color, IsArchived = $isArchived
				WHERE Id = $id;
				""",
				("$id", id.ToString("D")),
				("$name", name.Trim()),
				("$emoji", string.IsNullOrWhiteSpace(emoji) ? null : emoji.Trim()),
				("$color", color.Trim()),
				("$isArchived", isArchived ? 1 : 0));

			return command.ExecuteNonQuery() == 1;
		}, cancellationToken);

	public Task<bool> DeleteAsync(
		Guid id,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"DELETE FROM Category WHERE Id = $id;",
				("$id", id.ToString("D")));

			return command.ExecuteNonQuery() == 1;
		}, cancellationToken);
}
