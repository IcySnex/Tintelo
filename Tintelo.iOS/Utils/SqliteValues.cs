using System.Globalization;

namespace Tintelo.iOS.Utils;

public static class SqliteValues
{
	public static string ToText(
		DateOnly date) =>
		date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

	public static DateOnly ToDate(
		string text)
	{
		if (text.Length == 10 && text[4] == '-' && text[7] == '-')
		{
			int year = (text[0] - '0') * 1000 + (text[1] - '0') * 100 + (text[2] - '0') * 10 + text[3] - '0';
			int month = (text[5] - '0') * 10 + text[6] - '0';
			int day = (text[8] - '0') * 10 + text[9] - '0';

			return new DateOnly(year, month, day);
		}

		return DateOnly.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
	}

	
	public static string ToUtcText(
		DateTime value) =>
		value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);

	public static DateTime ToUtc(
		string text) =>
		DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToUniversalTime();
}
