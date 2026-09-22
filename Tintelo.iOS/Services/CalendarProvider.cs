using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Tintelo.iOS.Models;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public class CalendarProvider : ObservableObject
{
	readonly ILogger<CalendarProvider> logger;
	readonly AppConfig config;
	
	readonly Dictionary<YearMonth, CalendarMonthSummary> months = []; // The same month instances the view model holds, so in-place item updates reach the calendar.
	readonly Dictionary<DateOnly, CalendarDaySummary> days = []; // Day content until SQLite exists; new months are filled from it.
	
	public CalendarProvider(
		ILogger<CalendarProvider> logger,
		AppConfig config)
	{
		this.logger = logger;
		this.config = config;

		config.Calendar.PropertyChanged += (_, e) =>
		{
			if (e.PropertyName == nameof(config.Calendar.FirstDayOfWeek))
			{
				logger.LogInformation("First day of week changed to {Day}.", config.Calendar.FirstDayOfWeek);
				
				foreach (CalendarMonthSummary month in months.Values)
					month.SetLeading(Leading(month.Key));
				
				OnPropertyChanged(nameof(FirstWeekday));
			}
		};
	}

	
	CalendarMonthSummary CreateMonth(
		YearMonth month)
	{
		int leading = Leading(month);
		int dayCount = DateTime.DaysInMonth(month.Year, month.Month);
		
		List<ICalendarDaySummary> items = new(leading + dayCount);
		
		for (int index = 0; index < leading; index++)
			items.Add(new EmptyCalendarDaySummary());

		for (int day = 1; day <= dayCount; day++)
		{
			DateOnly date = new(month.Year, month.Month, day);
			
			items.Add(days.TryGetValue(date, out CalendarDaySummary? saved)
				? saved with { Key = date }
				: new CalendarDaySummary(date, null, false));
		}
		
		return new(month, new ObservableRangeCollection<ICalendarDaySummary>(items));
	}

	int Leading(
		YearMonth month)
	{
		DateOnly firstDay = new(month.Year, month.Month, 1);
		return ((int)firstDay.DayOfWeek - (int)FirstWeekday + 7) % 7;
	}
	
	
	public DayOfWeek FirstWeekday => config.Calendar.FirstDayOfWeek switch
	{
		FirstDayOfWeek.Automatic => CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek,
		FirstDayOfWeek.Monday => DayOfWeek.Monday,
		FirstDayOfWeek.Sunday => DayOfWeek.Sunday,
		_ => throw new ArgumentOutOfRangeException()
	};

	
	public CalendarMonthSummary GetMonth(
		YearMonth month)
	{
		if (months.TryGetValue(month, out CalendarMonthSummary? cached))
			return cached;
		
		CalendarMonthSummary created = CreateMonth(month);
		months.Add(month, created);
		
		return created;
	}
	
	public void SetDay(
		DateOnly date,
		Mood? mood,
		bool hasNote)
	{
		days[date] = new(date, mood, hasNote);
		
		if (!months.TryGetValue(YearMonth.From(date), out CalendarMonthSummary? month))
			return;
		
		month.SetItem(Leading(month.Key) + date.Day - 1, new CalendarDaySummary(date, mood, hasNote));
	}
}
