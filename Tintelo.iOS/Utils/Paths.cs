namespace Tintelo.iOS.Utils;

public static class Paths
{
	static readonly string Application = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

	static readonly string Cache = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Cache"); // DEBUG PURPOSES


	public static readonly string Logs = Path.Combine(Cache, "Logs");
}
