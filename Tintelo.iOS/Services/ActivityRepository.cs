using Microsoft.Data.Sqlite;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public sealed class ActivityRepository(
	Database database)
{
	const string SelectAll = "SELECT Id, Name, Icon, Color, IsArchived FROM Activity ORDER BY Name;";
	const string SelectActive = "SELECT Id, Name, Icon, Color, IsArchived FROM Activity WHERE IsArchived = 0 ORDER BY Name;";

	
	static string SerializeIds(
		IReadOnlyCollection<Guid> ids) =>
		$"[{string.Join(",", ids.Select(id => $"\"{id:D}\""))}]";


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

			using SqliteCommand command = connection.CreateCommand(
				transaction,
				"""
				INSERT INTO Activity (Id, Name, Icon, Color, IsArchived)
				VALUES ($id, $name, $icon, $color, 0);
				""",
				("$id", id.ToString("D")),
				("$name", trimmedName),
				("$icon", icon?.ToString()),
				("$color", trimmedColor));

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
