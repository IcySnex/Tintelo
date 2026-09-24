using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public class CalendarProvider : ObservableObject
{
	static int GetMonthLeadingItemCount(
		YearMonth month,
		DayOfWeek firstWeekday)
	{
		DateOnly firstDay = new(month.Year, month.Month, 1);
		return ((int)firstDay.DayOfWeek - (int)firstWeekday + 7) % 7;
	}

	public static int GetMonthItemCount(
		YearMonth month,
		DayOfWeek firstWeekday) =>
		GetMonthLeadingItemCount(month, firstWeekday) + DateTime.DaysInMonth(month.Year, month.Month);
	
	
	readonly AppConfig config;
	
	readonly Dictionary<YearMonth, CalendarMonthSummary> months = []; // The same month instances the view model holds, so in-place item updates reach the calendar.
	readonly Dictionary<DateOnly, CalendarDaySummary> days = []; // Day content until SQLite exists; new months are filled from it.
	readonly LinkedList<YearMonth> monthOrder = []; // Least recently shown first, so the cache stays bounded.

	const int MonthCacheLimit = 24;
	
	public CalendarProvider(
		ILogger<CalendarProvider> logger,
		AppConfig config)
	{
		this.config = config;
		
		Months = new CalendarMonthSource(this);

		config.Calendar.PropertyChanged += (_, e) =>
		{
			if (e.PropertyName == nameof(config.Calendar.FirstDayOfWeek))
			{
				logger.LogInformation("First day of week changed to {Day}.", config.Calendar.FirstDayOfWeek);
				
				foreach (CalendarMonthSummary month in months.Values)
					month.SetLeading(GetMonthLeadingItemCount(month.Key, FirstWeekday));
				
				OnPropertyChanged(nameof(FirstWeekday));
			}
		};
	}

	
	CalendarMonthSummary CreateMonth(
		YearMonth month)
	{
		int year = month.Year;
		int monthNumber = month.Month;
		
		int leadingCount = GetMonthLeadingItemCount(month, FirstWeekday);
		int dayCount = DateTime.DaysInMonth(month.Year, month.Month);
		
		ICalendarDaySummary[] items = new ICalendarDaySummary[leadingCount + dayCount];
		Array.Fill(items, ICalendarDaySummary.Empty, 0, leadingCount);

		for (int day = 1; day <= dayCount; day++)
		{
			DateOnly date = new(year, monthNumber, day);
			items[leadingCount + day - 1] = days.TryGetValue(date, out CalendarDaySummary? saved) 
				? saved
				: new CalendarDaySummary(date, null, false, false);
		}
		
		return new(month, new ObservableRangeCollection<ICalendarDaySummary>(items));
	}

	
	public DayOfWeek FirstWeekday => config.Calendar.FirstDayOfWeek switch
	{
		FirstDayOfWeek.Automatic => CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek,
		FirstDayOfWeek.Monday => DayOfWeek.Monday,
		FirstDayOfWeek.Sunday => DayOfWeek.Sunday,
		_ => throw new ArgumentOutOfRangeException()
	};

	public IVirtualizedList<CalendarMonthSummary> Months { get; }
	

	public CalendarMonthSummary GetMonth(
		YearMonth month)
	{
		if (months.TryGetValue(month, out CalendarMonthSummary? cached))
		{
			monthOrder.Remove(month);
			monthOrder.AddFirst(month);
			return cached;
		}
		
		CalendarMonthSummary created = CreateMonth(month);
		months.Add(month, created);
		monthOrder.AddFirst(month);
		
		while (months.Count > MonthCacheLimit)
		{
			YearMonth oldest = monthOrder.Last!.Value;
			monthOrder.RemoveLast();
			months.Remove(oldest);
		}
		
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
		
		month.SetItem(GetMonthLeadingItemCount(month.Key, FirstWeekday) + date.Day - 1, new CalendarDaySummary(date, mood, hasNote));
	}
}
