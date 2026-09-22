using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels.Calendar;

public partial class CalendarViewModel(
	INavigator navigator) : ObservableObject
{
	public CalendarMonthSummary[] Months { get; } = new[]
	{
		new CalendarMonthSummary(
			new YearMonth(2026, 9),
			new ICalendarDaySummary[]
			{
				new EmptyCalendarDaySummary(),
				new EmptyCalendarDaySummary(),
				new CalendarDaySummary(new(2026, 9, 1), null, false),
				new CalendarDaySummary(new(2026, 9, 2), null, false),
				new CalendarDaySummary(new(2026, 9, 3), null, false),
				new CalendarDaySummary(new(2026, 9, 4), null, false),
				new CalendarDaySummary(new(2026, 9, 5), null, false),
				new CalendarDaySummary(new(2026, 9, 6), Mood.ExtremelyBad, true),
				new CalendarDaySummary(new(2026, 9, 7), Mood.VeryBad, true),
				new CalendarDaySummary(new(2026, 9, 8), Mood.Bad, true),
				new CalendarDaySummary(new(2026, 9, 9), Mood.Neutral, true),
				new CalendarDaySummary(new(2026, 9, 10), Mood.Good, true),
				new CalendarDaySummary(new(2026, 9, 11), Mood.VeryGood, true),
				new CalendarDaySummary(new(2026, 9, 12), Mood.ExtremelyGood, true),
				new CalendarDaySummary(new(2026, 9, 13), null, false),
				new CalendarDaySummary(new(2026, 9, 14), null, false),
				new CalendarDaySummary(new(2026, 9, 15), null, false),
				new CalendarDaySummary(new(2026, 9, 16), null, false),
				new CalendarDaySummary(new(2026, 9, 17), null, false),
				new CalendarDaySummary(new(2026, 9, 18), null, false),
				new CalendarDaySummary(new(2026, 9, 19), null, false),
				new CalendarDaySummary(new(2026, 9, 20), null, false),
				new CalendarDaySummary(new(2026, 9, 21), null, false),
				new CalendarDaySummary(new(2026, 9, 22), null, false),
				new CalendarDaySummary(new(2026, 9, 23), null, false),
				new CalendarDaySummary(new(2026, 9, 24), null, false),
				new CalendarDaySummary(new(2026, 9, 25), null, false),
				new CalendarDaySummary(new(2026, 9, 26), null, false),
				new CalendarDaySummary(new(2026, 9, 27), null, false),
				new CalendarDaySummary(new(2026, 9, 28), null, false),
				new CalendarDaySummary(new(2026, 9, 29), null, false),
				new CalendarDaySummary(new(2026, 9, 30), null, false)
			})
	};
	
	
	[RelayCommand]
	Task OpenSettingsAsync() =>
		navigator.PresentAsync<SettingsViewModel>(ModalStyle.FormSheet);


	[RelayCommand]
	Task OpenDetailsAsync(
		CalendarDaySummary summary)
	{
		// tell details view model selected day has changed (if needed)
		// if sidebar is available and closed, open it
		// else open details modal with selected day
		
		return Task.CompletedTask;
	}
}
