namespace Tintelo.iOS.Models.Calendar;

public readonly record struct YearMonth(
	int Year,
	int Month) : IComparable<YearMonth>
{
	public static YearMonth Current =>
		From(DateTime.Now);
	
	
	public static YearMonth From(
		DateOnly date) =>
		new(date.Year, date.Month);
	
	public static YearMonth From(
		DateTime dateTime) =>
		new(dateTime.Year, dateTime.Month);
	
	
	public YearMonth Add(
		int months)
	{
		int total = checked(Year * 12 + Month - 1 + months);
		int year = Math.DivRem(total, 12, out int month);
		
		if (month < 0)
		{
			month += 12;
			year -= 1;
		}

		return new(year, month + 1);
	}

	public int CompareTo(
		YearMonth other)
	{
		int year = Year.CompareTo(other.Year);
		return year != 0 ? year : Month.CompareTo(other.Month);
	}

	public static bool operator <(YearMonth left, YearMonth right) =>
		left.CompareTo(right) < 0;

	public static bool operator <=(YearMonth left, YearMonth right) =>
		left.CompareTo(right) <= 0;

	public static bool operator >(YearMonth left, YearMonth right) =>
		left.CompareTo(right) > 0;

	public static bool operator >=(YearMonth left, YearMonth right) =>
		left.CompareTo(right) >= 0;


	public override string ToString() =>
		$"{Year:D4}-{Month:D2}";
}
