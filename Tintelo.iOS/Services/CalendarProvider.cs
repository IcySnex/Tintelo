using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Journal;
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
	
	
	readonly ILogger<CalendarProvider> logger;
	readonly AppConfig config;
	readonly EntryRepository entries;
	
	readonly Dictionary<YearMonth, CalendarMonthSummary> months = []; // The same month instances the view model holds, so in-place item updates reach the calendar.
	readonly Dictionary<DateOnly, CalendarDaySummary> days = []; // Day content loaded from the repository, so evicted months are filled without another query.
	readonly HashSet<YearMonth> loadedMonths = [];
	readonly HashSet<YearMonth> loadingMonths = [];
	readonly LinkedList<YearMonth> monthOrder = []; // Least recently shown first, so the cache stays bounded.

	int generation;

	const int MonthCacheLimit = 24;
	
	public CalendarProvider(
		ILogger<CalendarProvider> logger,
		AppConfig config,
		EntryRepository entries)
	{
		this.logger = logger;
		this.config = config;
		this.entries = entries;
		
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
		bool loaded = loadedMonths.Contains(month);
		
		ICalendarDaySummary[] items = new ICalendarDaySummary[leadingCount + dayCount];
		Array.Fill(items, ICalendarDaySummary.Empty, 0, leadingCount);

		for (int day = 1; day <= dayCount; day++)
		{
			DateOnly date = new(year, monthNumber, day);
			items[leadingCount + day - 1] = days.TryGetValue(date, out CalendarDaySummary? saved) 
				? saved
				: new CalendarDaySummary(date, null, false, loaded);
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
		EnsureMonthLoaded(month);
		
		while (months.Count > MonthCacheLimit)
		{
			YearMonth oldest = monthOrder.Last!.Value;
			monthOrder.RemoveLast();
			months.Remove(oldest);
		}
		
		return created;
	}
	
	void EnsureMonthLoaded(
		YearMonth month)
	{
		if (loadedMonths.Contains(month) || !loadingMonths.Add(month))
			return;
		
		_ = LoadMonthAsync(month, generation);
	}

	async Task LoadMonthAsync(
		YearMonth month,
		int loadGeneration)
	{
		try
		{
			DateOnly from = new(month.Year, month.Month, 1);
			IReadOnlyList<EntrySummary> summaries = await entries.GetSummariesAsync(from, from.AddMonths(1).AddDays(-1));
			if (loadGeneration != generation)
				return;

			foreach (EntrySummary summary in summaries)
				SetDay(summary.Date, summary.Mood, summary.HasNote);
			
			loadedMonths.Add(month);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Loading journal entries for {Month} failed.", month);
		}
		finally
		{
			if (loadGeneration == generation)
				loadingMonths.Remove(month);
		}
	}

	public async Task ReloadAsync()
	{
		generation++;
		loadedMonths.Clear();
		loadingMonths.Clear();
		days.Clear();

		CalendarMonthSummary[] cached = [.. months.Values];
		if (cached.Length == 0)
			return;

		YearMonth first = cached.MinBy(month => month.Key.Year * 12 + month.Key.Month)!.Key;
		YearMonth last = cached.MaxBy(month => month.Key.Year * 12 + month.Key.Month)!.Key;
		DateOnly from = new(first.Year, first.Month, 1);
		DateOnly to = new DateOnly(last.Year, last.Month, 1).AddMonths(1).AddDays(-1);

		IReadOnlyList<EntrySummary> summaries = await entries.GetSummariesAsync(from, to);

		foreach (EntrySummary summary in summaries)
			days[summary.Date] = new CalendarDaySummary(summary.Date, summary.Mood, summary.HasNote);

		foreach (CalendarMonthSummary month in cached)
		{
			loadedMonths.Add(month.Key);

			for (int day = 1; day <= DateTime.DaysInMonth(month.Key.Year, month.Key.Month); day++)
			{
				DateOnly date = new(month.Key.Year, month.Key.Month, day);

				month.SetItem(
					GetMonthLeadingItemCount(month.Key, FirstWeekday) + day - 1,
					days.TryGetValue(date, out CalendarDaySummary? summary)
						? summary
						: new CalendarDaySummary(date, null, false));
			}
		}
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
