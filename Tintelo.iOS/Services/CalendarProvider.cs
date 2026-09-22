using System.Globalization;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.Services;

public sealed class CalendarProvider(
	AppConfig config)
{
	public DayOfWeek GetFirstWeekday() =>
		config.Calendar.FirstDayOfWeek switch
		{
			FirstDayOfWeek.Automatic => CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek,
			FirstDayOfWeek.Monday => DayOfWeek.Monday,
			FirstDayOfWeek.Sunday => DayOfWeek.Sunday,
			_ => throw new ArgumentOutOfRangeException(nameof(config.Calendar.FirstDayOfWeek))
		};
	
	
	public CalendarMonthSummary GetMonth(
		YearMonth month)
	{
		DateOnly firstDay = new(month.Year, month.Month, 1);
		int leading = ((int)firstDay.DayOfWeek - (int)GetFirstWeekday() + 7) % 7;
		int days = DateTime.DaysInMonth(month.Year, month.Month);
		
		List<ICalendarDaySummary> items = new(leading + days);
		
		for (int index = 0; index < leading; index++)
			items.Add(new EmptyCalendarDaySummary());

		for (int day = 1; day <= days; day++)
			items.Add(new CalendarDaySummary(new(month.Year, month.Month, day), null, false));
		
		return new(month, items);
	}
}
