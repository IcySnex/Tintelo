using Tintelo.iOS.Models.Journal;

namespace Tintelo.iOS.Models.Calendar;

public interface ICalendarDaySummary
{
	public static readonly EmptyCalendarDaySummary Empty = new();
}

public sealed record CalendarDaySummary(
	DateOnly Key,
	Mood? Mood,
	bool HasNote) : ICalendarDaySummary;

public sealed record EmptyCalendarDaySummary : ICalendarDaySummary;
