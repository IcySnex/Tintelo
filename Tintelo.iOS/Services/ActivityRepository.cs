using Microsoft.Data.Sqlite;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public sealed class ActivityRepository(
	Database database)
{
	const string SelectAll = "SELECT Id, Name, Icon, Color, IsArchived FROM Activity ORDER BY IsArchived, SortOrder, Name;";
	const string SelectActive = "SELECT Id, Name, Icon, Color, IsArchived FROM Activity WHERE IsArchived = 0 ORDER BY SortOrder, Name;";

	
	static string SerializeIds(
		IReadOnlyCollection<Guid> ids) =>
		$"[{string.Join(",", ids.Select(id => $"\"{id:D}\""))}]";

	static string? ReadSortOrder(
		SqliteConnection connection,
		SqliteTransaction transaction,
		Guid id)
	{
		using SqliteCommand command = connection.CreateCommand(
			transaction,
			"SELECT SortOrder FROM Activity WHERE Id = $id;",
			("$id", id.ToString("D")));

		return command.ExecuteScalar() as string;
	}

	static string? ReadLastSortOrder(
		SqliteConnection connection,
		SqliteTransaction transaction)
	{
		using SqliteCommand command = connection.CreateCommand(
			transaction,
			"SELECT SortOrder FROM Activity ORDER BY SortOrder DESC LIMIT 1;");

		return command.ExecuteScalar() as string;
	}


	internal static Activity ReadActivity(
		SqliteDataReader reader) =>
		new(
			reader.GetGuid(0),
			reader.GetString(1),
			reader.IsDBNull(2) ? null : reader.GetString(2).ToActivityIcon(),
			reader.GetString(3),
			reader.GetBoolean(4));


	// actions
	public Task<IReadOnlyList<Activity>> GetAsync(
		bool includeArchived = false,
		CancellationToken cancellationToken = default) =>
		database.ReadAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				includeArchived ? SelectAll : SelectActive);

			using SqliteDataReader reader = command.ExecuteReader();

			List<Activity> activities = [];
			while (reader.Read())
				activities.Add(ReadActivity(reader));

			return (IReadOnlyList<Activity>)activities;
		}, cancellationToken);

	
	public Task<Activity> CreateAsync(
		string name,
		ActivityIcon? icon,
		string color,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			Guid id = Guid.CreateVersion7();
			string trimmedName = name.Trim();
			string trimmedColor = color.Trim();
			string sortOrder = FractionalIndex.Between(ReadLastSortOrder(connection, transaction), null);

			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				INSERT INTO Activity (Id, Name, Icon, Color, IsArchived, SortOrder)
				VALUES ($id, $name, $icon, $color, 0, $sortOrder);
				""",
				("$id", id.ToString("D")),
				("$name", trimmedName),
				("$icon", icon?.ToString()),
				("$color", trimmedColor),
				("$sortOrder", sortOrder));

			command.ExecuteNonQuery();

			return new Activity(id, trimmedName, icon, trimmedColor, false);
		}, cancellationToken);

	
	public Task<bool> UpdateAsync(
		Guid id,
		string name,
		ActivityIcon? icon,
		string color,
		bool isArchived,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				UPDATE Activity
				SET Name = $name, Icon = $icon, Color = $color, IsArchived = $isArchived
				WHERE Id = $id;
				""",
				("$id", id.ToString("D")),
				("$name", name.Trim()),
				("$icon", icon?.ToString()),
				("$color", color.Trim()),
				("$isArchived", isArchived ? 1 : 0));

			return command.ExecuteNonQuery() == 1;
		}, cancellationToken);

	
	public Task<bool> MoveAsync(
		Guid id,
		Guid? previousId,
		Guid? nextId,
		bool isArchived,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			string? previous = previousId is Guid previousKey
				? ReadSortOrder(connection, transaction, previousKey)
				: null;

			string? next = nextId is Guid nextKey
				? ReadSortOrder(connection, transaction, nextKey)
				: null;

			if (previousId is not null && previous is null)
				throw new InvalidOperationException("The previous activity no longer exists.");

			if (nextId is not null && next is null)
				throw new InvalidOperationException("The next activity no longer exists.");

			string sortOrder = previous is null && next is null
				? FractionalIndex.Between(ReadLastSortOrder(connection, transaction), null)
				: FractionalIndex.Between(previous, next);

			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				UPDATE Activity
				SET IsArchived = $isArchived, SortOrder = $sortOrder
				WHERE Id = $id;
				""",
				("$id", id.ToString("D")),
				("$isArchived", isArchived ? 1 : 0),
				("$sortOrder", sortOrder));

			return command.ExecuteNonQuery() == 1;
		}, cancellationToken);


	public Task<int> SetArchivedAsync(
		IReadOnlyCollection<Guid> ids,
		bool isArchived,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				UPDATE Activity
				SET IsArchived = $isArchived
				WHERE Id IN (SELECT value FROM json_each($ids));
				""",
				("$isArchived", isArchived ? 1 : 0),
				("$ids", SerializeIds(ids)));

			return command.ExecuteNonQuery();
		}, cancellationToken);

	public async Task<bool> SetArchivedAsync(
		Guid id,
		bool isArchived,
		CancellationToken cancellationToken = default) =>
		await SetArchivedAsync([id], isArchived, cancellationToken).ConfigureAwait(false) == 1;
	
	
	public Task<int> DeleteAsync(
		IReadOnlyCollection<Guid> ids,
		CancellationToken cancellationToken = default) =>
		database.WriteAsync((connection, transaction) =>
		{
			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"DELETE FROM Activity WHERE Id IN (SELECT value FROM json_each($ids));",
				("$ids", SerializeIds(ids)));

			return command.ExecuteNonQuery();
		}, cancellationToken);

	public async Task<bool> DeleteAsync(
		Guid id,
		CancellationToken cancellationToken = default) =>
		await DeleteAsync([id], cancellationToken).ConfigureAwait(false) == 1;
}
