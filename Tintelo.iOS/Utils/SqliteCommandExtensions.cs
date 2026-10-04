using Microsoft.Data.Sqlite;

namespace Tintelo.iOS.Utils;

public static class SqliteCommandExtensions
{
	extension(SqliteConnection connection)
	{
		public SqliteCommand CreateCommand(
			SqliteTransaction transaction,
			string text,
			params (string Name, object? Value)[] parameters)
		{
			SqliteCommand command = connection.CreateCommand();
			command.Transaction = transaction;
			command.CommandText = text;

			foreach ((string name, object? value) in parameters)
				command.Parameters.AddWithValue(name, value ?? DBNull.Value);

			return command;
		}
	}
}
