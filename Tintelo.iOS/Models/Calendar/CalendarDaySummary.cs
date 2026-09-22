namespace Tintelo.iOS.Models.Calendar;

public interface ICalendarDaySummary;

public sealed record CalendarDaySummary(
	DateOnly Key,
	Mood? Mood,
	bool HasNote) : ICalendarDaySummary;

public sealed record EmptyCalendarDaySummary : ICalendarDaySummary;
