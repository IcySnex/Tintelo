using System.Globalization;
using SkeleKit;
using Tintelo.iOS.Models.Journal;

namespace Tintelo.iOS.Utils;

public static class StringExtensions
{
	extension(string value)
	{
		public string Format(
			params object?[] args) =>
			string.Format(CultureInfo.CurrentCulture, value, args);

		public Color ToColor(
			double alpha = 1.0)
		{
			uint hex = uint.Parse(value.AsSpan(1), NumberStyles.HexNumber);
			return new Color(
				(hex >> 16 & 0xFF) / 255.0,
				(hex >> 8 & 0xFF) / 255.0,
				(hex >> 0 & 0xFF) / 255.0,
				alpha);
		}
	}

	extension(string? value)
	{
		public ActivityIcon? ToActivityIcon()
		{
			if (string.IsNullOrWhiteSpace(value))
				return null;

			string key = value.Trim();

			if (int.TryParse(key, out _))
				return null;

			return Enum.TryParse(key, true, out ActivityIcon icon) && Enum.IsDefined(icon)
				? icon
				: null;
		}
	}
}
