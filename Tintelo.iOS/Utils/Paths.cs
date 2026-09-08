namespace Tintelo.iOS.Utils;

public static class Paths
{
	static string GetDirectoryPath(
		NSSearchPathDirectory directory) =>
		NSFileManager.DefaultManager
			.GetUrls(directory, NSSearchPathDomain.User)
			.Single()
			.Path!;
	
	static readonly string Documents = GetDirectoryPath(NSSearchPathDirectory.DocumentDirectory);
	static readonly string Caches = GetDirectoryPath(NSSearchPathDirectory.CachesDirectory);
	static readonly string ApplicationSupport = GetDirectoryPath(NSSearchPathDirectory.ApplicationSupportDirectory);
	

	public static readonly string Logs = Path.Combine(Caches, "Logs");
	public static readonly string Database = Path.Combine(ApplicationSupport, "Tintelo.sqlite3");
}
