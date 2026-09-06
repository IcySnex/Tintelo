using System.Globalization;

namespace Tintelo.iOS.Utils;

public static class StringExtensions
{
	extension(string value)
	{
		public string Format(
			params object?[] args) =>
			string.Format(CultureInfo.CurrentCulture, value, args);
	}
}
