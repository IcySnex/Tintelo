namespace Tintelo.iOS.Models.Calendar;

public interface ICalendarDaySummary;

public record CalendarDaySummary(
	int Day,
	Mood? Mood,
	bool HasNote) : ICalendarDaySummary;
	
public record EmptyCalendarDaySummary : ICalendarDaySummary;