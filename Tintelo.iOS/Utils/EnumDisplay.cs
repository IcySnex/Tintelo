namespace Tintelo.iOS.Utils;

public sealed class EnumDisplay<TEnum>
	where TEnum : struct, Enum
{
	readonly TEnum[] values = Enum.GetValues<TEnum>();
	readonly string[] titles;
	readonly string?[] descriptions;
	
	public EnumDisplay(
		Func<TEnum, string> title,
		Func<TEnum, string?>? description = null)
	{
		ArgumentNullException.ThrowIfNull(title);

		titles = new string[values.Length];
		descriptions = new string?[values.Length];

		for (int index = 0; index < values.Length; index++)
		{
			string resolved = title(Values[index]);
			if (string.IsNullOrEmpty(resolved))
				throw new ArgumentException($"Every {typeof(TEnum).Name} value needs a non-empty title.", nameof(title));

			titles[index] = resolved;
			descriptions[index] = description?.Invoke(values[index]);
		}

		if (titles.Distinct(StringComparer.Ordinal).Count() != titles.Length)
			throw new ArgumentException($"Every {typeof(TEnum).Name} value needs a unique title.", nameof(title));
	}


	public IReadOnlyList<TEnum> Values => values;
	
	public IReadOnlyList<string> Titles => titles;
	
	public IReadOnlyList<string?> Descriptions => descriptions;

	
	public string GetTitle(
		TEnum value) =>
		Titles[GetIndex(value)];

	public string? GetDescription(
		TEnum value) =>
		Descriptions[GetIndex(value)];
	
	public TEnum GetValue(
		string? title)
	{
		int index = Array.IndexOf(titles, title);
		if (index < 0)
			throw new ArgumentOutOfRangeException(nameof(title), title, $"'{title}' is not a title of {typeof(TEnum).Name}.");

		return Values[index];
	}
	
	public int GetIndex(
		TEnum value)
	{
		int index = Array.IndexOf(values, value);
		if (index < 0)
			throw new ArgumentOutOfRangeException(nameof(value), value, $"{value} is not a defined {typeof(TEnum).Name} value.");

		return index;
	}
}
