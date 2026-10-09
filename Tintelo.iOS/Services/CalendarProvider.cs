using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CoreFoundation;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Services;

public class CalendarProvider : ObservableObject
{
	const int RecentMonths = 24;
	const int MonthCacheLimit = 24;

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

	static DateOnly FirstDay(
		YearMonth month) =>
		new(month.Year, month.Month, 1);

	static DateOnly LastDay(
		YearMonth month) =>
		FirstDay(month).AddMonths(1).AddDays(-1);

	static Dictionary<DateOnly, EntrySummary> ToDays(
		IReadOnlyList<EntrySummary> summaries)
	{
		Dictionary<DateOnly, EntrySummary> loaded = new(summaries.Count);

		foreach (EntrySummary summary in summaries)
			loaded[summary.Date] = summary;

		return loaded;
	}

	static Task OnMainAsync(
		Action action)
	{
		if (NSThread.IsMain)
		{
			action();
			return Task.CompletedTask;
		}

		TaskCompletionSource completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

		DispatchQueue.MainQueue.DispatchAsync(() =>
		{
			try
			{
				action();
				completion.SetResult();
			}
			catch (Exception exception)
			{
				completion.SetException(exception);
			}
		});

		return completion.Task;
	}


	readonly ILogger<CalendarProvider> logger;
	readonly AppConfig config;
	readonly EntryRepository entries;

	readonly Dictionary<YearMonth, CalendarMonthSummary> months = [];
	readonly LinkedList<YearMonth> monthOrder = [];

	Dictionary<DateOnly, EntrySummary> days = [];
	(YearMonth From, YearMonth To)? resolved;

	Task? initialLoad;
	Task? fullLoad;

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


	ICalendarDaySummary[] CreateItems(
		YearMonth month)
	{
		int leadingCount = GetMonthLeadingItemCount(month, FirstWeekday);
		int dayCount = DateTime.DaysInMonth(month.Year, month.Month);

		ICalendarDaySummary[] items = new ICalendarDaySummary[leadingCount + dayCount];
		Array.Fill(items, ICalendarDaySummary.Empty, 0, leadingCount);

		for (int day = 1; day <= dayCount; day++)
		{
			DateOnly date = new(month.Year, month.Month, day);
			items[leadingCount + day - 1] = days.TryGetValue(date, out EntrySummary summary)
				? new CalendarDaySummary(date, summary.Mood, summary.HasNote)
				: new CalendarDaySummary(date, null, false);
		}

		return items;
	}

	CalendarMonthSummary CreateMonth(
		YearMonth month) =>
		new(month, new ObservableRangeCollection<ICalendarDaySummary>(CreateItems(month)));

	
	async Task LoadInitialAsync()
	{
		YearMonth to = YearMonth.Current;
		YearMonth from = to.Add(-(RecentMonths - 1));

		try
		{
			Stopwatch watch = Stopwatch.StartNew();
			IReadOnlyList<EntrySummary> summaries = await entries
				.GetSummariesAsync(FirstDay(from), LastDay(to))
				.ConfigureAwait(false);
			watch.Stop();

			days = ToDays(summaries);
			resolved = (from, to);

			logger.LogInformation(
				"Loaded {Count} recent journal summaries for {From}..{To} in {Elapsed} ms.",
				summaries.Count, from, to, watch.ElapsedMilliseconds);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Loading recent journal summaries failed.");
		}

		fullLoad = LoadAllAsync();
	}

	async Task LoadAllAsync()
	{
		try
		{
			await ReadAllAsync(refreshAll: false).ConfigureAwait(false);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Loading all journal summaries failed.");
		}
	}

	async Task ReadAllAsync(
		bool refreshAll)
	{
		Stopwatch watch = Stopwatch.StartNew();
		IReadOnlyList<EntrySummary> summaries = await entries.GetSummariesAsync().ConfigureAwait(false);
		Dictionary<DateOnly, EntrySummary> loaded = ToDays(summaries);

		await OnMainAsync(() =>
		{
			days = loaded;

			foreach (CalendarMonthSummary month in months.Values)
			{
				if (refreshAll || !(resolved is { } range && month.Key >= range.From && month.Key <= range.To))
					month.SetItems(CreateItems(month.Key));
			}

			resolved = null;
		}).ConfigureAwait(false);

		watch.Stop();

		logger.LogInformation(
			"Loaded {Count} journal summaries in {Elapsed} ms.",
			summaries.Count, watch.ElapsedMilliseconds);
	}


	public DayOfWeek FirstWeekday => config.Calendar.FirstDayOfWeek switch
	{
		FirstDayOfWeek.Automatic => CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek,
		FirstDayOfWeek.Monday => DayOfWeek.Monday,
		FirstDayOfWeek.Sunday => DayOfWeek.Sunday,
		_ => throw new ArgumentOutOfRangeException()
	};

	public IVirtualizedList<CalendarMonthSummary> Months { get; }


	public Task LoadAsync() =>
		initialLoad ??= LoadInitialAsync();

	public async Task ReloadAsync()
	{
		if (fullLoad is { } pending)
			await pending.ConfigureAwait(false);

		try
		{
			await ReadAllAsync(refreshAll: true).ConfigureAwait(false);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "Reloading journal summaries failed.");
		}
	}

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
		if (mood is Mood value)
			days[date] = new(date, value, hasNote);
		else
			days.Remove(date);

		if (!months.TryGetValue(YearMonth.From(date), out CalendarMonthSummary? month))
			return;

		month.SetItem(GetMonthLeadingItemCount(month.Key, FirstWeekday) + date.Day - 1, new CalendarDaySummary(date, mood, hasNote));
	}
}
