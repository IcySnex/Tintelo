namespace Tintelo.iOS.Utils;

public static class FractionalIndex
{
	const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
	const string IntDigits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
	const int HalfLength = 26;
	const char Zero = '0';

	static readonly int[] DigitLookup = CreateLookup(Digits);
	static readonly int[] IntDigitLookup = CreateLookup(IntDigits);
	static readonly string SmallestInteger = string.Concat(IntDigits[0], new string(Zero, HalfLength));


	public static string Between(
		string? lower,
		string? upper)
	{
		if (lower is not null && upper is not null && string.CompareOrdinal(lower, upper) >= 0)
			throw new ArgumentException("The lower sort key must be less than the upper sort key.", nameof(upper));

		if (lower is null)
		{
			if (upper is null)
				return string.Concat(IntDigits[HalfLength], Digits[0]);

			string integer = GetIntegerPart(upper);
			string fraction = upper[integer.Length..];

			if (IsSmallestInteger(integer))
			{
				if (fraction.Length == 0)
					throw new InvalidOperationException("No sort key can be created before the smallest key.");

				return integer + Midpoint("", fraction);
			}

			if (string.CompareOrdinal(integer, upper) < 0)
				return integer;

			string? decremented = DecrementInteger(integer);
			if (decremented is null)
				throw new InvalidOperationException("The sort order cannot be decremented any further.");

			return decremented == SmallestInteger
				? decremented + Midpoint("", "1")
				: decremented;
		}

		if (upper is null)
		{
			string integer = GetIntegerPart(lower);
			string fraction = lower[integer.Length..];

			return IncrementInteger(integer)
				?? integer + Midpoint(fraction, null);
		}

		string lowerInteger = GetIntegerPart(lower);
		string lowerFraction = lower[lowerInteger.Length..];
		string upperInteger = GetIntegerPart(upper);
		string upperFraction = upper[upperInteger.Length..];

		if (lowerInteger == upperInteger)
			return lowerInteger + Midpoint(lowerFraction, upperFraction);

		string incremented = IncrementInteger(lowerInteger)
			?? throw new InvalidOperationException("The sort order cannot be incremented any further.");

		return string.CompareOrdinal(incremented, upper) < 0
			? incremented
			: lowerInteger + Midpoint(lowerFraction, null);
	}


	static string Midpoint(
		string lower,
		string? upper)
	{
		if (upper is not null && string.CompareOrdinal(lower, upper) >= 0)
			throw new InvalidOperationException("The sort order bounds are not ordered.");

		if (lower.Length > 0 && lower[^1] == Zero || upper is { Length: > 0 } && upper[^1] == Zero)
			throw new InvalidOperationException("The sort order cannot end with the lowest digit.");

		if (upper is { Length: > 0 })
		{
			int length = 0;
			while (length < upper.Length
				&& (length < lower.Length ? lower[length] : Zero) == upper[length])
				length++;

			if (length > 0)
			{
				return upper[..length] + Midpoint(
					length < lower.Length ? lower[length..] : "",
					length < upper.Length ? upper[length..] : null);
			}
		}

		int lowerDigit = lower.Length > 0 ? ValueOf(lower[0], DigitLookup) : 0;
		int upperDigit = upper is { Length: > 0 } ? ValueOf(upper[0], DigitLookup) : Digits.Length;

		if (lowerDigit < 0 || upperDigit < 0)
			throw new InvalidOperationException("The sort order contains an invalid digit.");

		if (upperDigit - lowerDigit > 1)
			return Digits[(lowerDigit + upperDigit) / 2].ToString();

		if (upper is { Length: > 1 })
			return upper[..1];

		return Digits[lowerDigit] + Midpoint(
			lower.Length > 0 ? lower[1..] : "",
			null);
	}


	static string GetIntegerPart(
		string key)
	{
		if (key.Length == 0)
			throw new ArgumentException("The sort key cannot be empty.", nameof(key));

		int length = GetIntegerLength(key[0]);
		if (length > key.Length)
			throw new ArgumentException("The sort key has an invalid integer part.", nameof(key));

		return key[..length];
	}

	static int GetIntegerLength(
		char head)
	{
		int index = ValueOf(head, IntDigitLookup);
		if (index < 0)
			throw new ArgumentException("The sort key has an invalid integer head.", nameof(head));

		return index < HalfLength
			? HalfLength - index + 1
			: index - HalfLength + 2;
	}

	static bool IsSmallestInteger(
		string integer) =>
		string.CompareOrdinal(integer, SmallestInteger) == 0;


	static string? IncrementInteger(
		string integer)
	{
		char head = integer[0];
		string trailing = "";

		for (int index = integer.Length - 1; index >= 1; index--)
		{
			int digit = ValueOf(integer[index], DigitLookup) + 1;
			if (digit == Digits.Length)
				trailing = Zero + trailing;
			else
				return head + integer[1..index] + Digits[digit] + trailing;
		}

		int headIndex = ValueOf(head, IntDigitLookup);
		if (headIndex == IntDigits.Length - 1)
			return null;

		char nextHead = IntDigits[headIndex + 1];
		int lengthDelta = GetIntegerLength(nextHead) - GetIntegerLength(head);
		string digits = lengthDelta > 0
			? trailing + Zero
			: lengthDelta < 0
				? trailing[1..]
				: trailing;

		return nextHead + digits;
	}

	static string? DecrementInteger(
		string integer)
	{
		char head = integer[0];
		char last = Digits[^1];
		string trailing = "";

		for (int index = integer.Length - 1; index >= 1; index--)
		{
			int digit = ValueOf(integer[index], DigitLookup) - 1;
			if (digit < 0)
				trailing = last + trailing;
			else
				return head + integer[1..index] + Digits[digit] + trailing;
		}

		int headIndex = ValueOf(head, IntDigitLookup);
		if (headIndex == 0)
			return null;

		char previousHead = IntDigits[headIndex - 1];
		int lengthDelta = GetIntegerLength(previousHead) - GetIntegerLength(head);
		string digits = lengthDelta > 0
			? trailing + last
			: lengthDelta < 0
				? trailing[1..]
				: trailing;

		return previousHead + digits;
	}


	static int[] CreateLookup(
		string alphabet)
	{
		int[] lookup = new int[128];
		Array.Fill(lookup, -1);

		for (int index = 0; index < alphabet.Length; index++)
			lookup[alphabet[index]] = index;

		return lookup;
	}

	static int ValueOf(
		char character,
		int[] lookup) =>
		character < lookup.Length ? lookup[character] : -1;
}
