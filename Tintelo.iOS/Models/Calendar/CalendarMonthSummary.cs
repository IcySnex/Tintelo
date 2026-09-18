using SkeleKit;

namespace Tintelo.iOS.Models.Calendar;

public record CalendarMonthSummary(
	int Month,
	int Year,
	IReadOnlyList<ICalendarDaySummary> Items) : ISection<ICalendarDaySummary>;