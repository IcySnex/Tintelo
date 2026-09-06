using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Tintelo.iOS.Logging;

internal static class LogEntryFormatter
{
	public static void Write(
		TextWriter writer,
		DateTimeOffset? timestamp,
		LogLevel level,
		string category,
		string message,
		Exception? exception)
	{
		writer.Write('[');
		if (timestamp is not null)
		{
			writer.Write(timestamp.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
			writer.Write(' ');
		}
		writer.Write(GetLevelName(level));
		writer.Write('-');
		writer.Write(GetClassName(category));
		writer.Write("] ");
		writer.WriteLine(message);

		if (exception is not null)
			writer.WriteLine(exception);
	}

	static string GetClassName(
		string category)
	{
		int separatorIndex = Math.Max(category.LastIndexOf('.'), category.LastIndexOf('+'));
		return category[(separatorIndex + 1)..];
	}

	static string GetLevelName(
		LogLevel level) => level switch
	{
		LogLevel.Trace => "TRC",
		LogLevel.Debug => "DBG",
		LogLevel.Information => "INF",
		LogLevel.Warning => "WRN",
		LogLevel.Error => "ERR",
		LogLevel.Critical => "CRT",
		_ => "NON"
	};
}
