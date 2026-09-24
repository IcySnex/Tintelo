namespace Tintelo.iOS.Models.Calendar;

public interface ICalendarDaySummary
{
	public static readonly EmptyCalendarDaySummary Empty = new();
}

public sealed record CalendarDaySummary(
	DateOnly Key,
	Mood? Mood,
	bool HasNote,
	bool IsResolved = true) : ICalendarDaySummary;

public sealed record EmptyCalendarDaySummary : ICalendarDaySummary;
