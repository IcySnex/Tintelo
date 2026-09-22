using SkeleKit;

namespace Tintelo.iOS.Models.Calendar;

public sealed record CalendarMonthSummary(
	YearMonth Key,
	IReadOnlyList<ICalendarDaySummary> Items) : ISection<ICalendarDaySummary>;