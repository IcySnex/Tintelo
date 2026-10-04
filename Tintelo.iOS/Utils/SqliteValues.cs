using System.Globalization;

namespace Tintelo.iOS.Utils;

public static class SqliteValues
{
	public static string ToText(
		DateOnly date) =>
		date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

	public static DateOnly ToDate(
		string text) =>
		DateOnly.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture);

	
	public static string ToUtcText(
		DateTime value) =>
		value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);

	public static DateTime ToUtc(
		string text) =>
		DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToUniversalTime();
}
