using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Logging;

public static class LogFiles
{
	public static string? GetLatestPath() =>
		Directory.Exists(Paths.Logs)
			? Directory
				.EnumerateFiles(Paths.Logs, "Log-*.txt")
				.OrderByDescending(Path.GetFileName, StringComparer.Ordinal)
				.FirstOrDefault()
			: null;
}
