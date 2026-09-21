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
		new CalendarMonthSummary(9, 2026, new ICalendarDaySummary[]
		{
			new EmptyCalendarDaySummary(),
			new EmptyCalendarDaySummary(),
			new CalendarDaySummary(1, null, false),
			new CalendarDaySummary(2, null, false),
			new CalendarDaySummary(3, null, false),
			new CalendarDaySummary(4, null, false),
			new CalendarDaySummary(5, null, false),
			new CalendarDaySummary(6, Mood.ExtremelyBad, true),
			new CalendarDaySummary(7, Mood.VeryBad, true),
			new CalendarDaySummary(8, Mood.Bad, true),
			new CalendarDaySummary(9, Mood.Neutral, true),
			new CalendarDaySummary(10, Mood.Good, true),
			new CalendarDaySummary(11, Mood.VeryGood, true),
			new CalendarDaySummary(12, Mood.ExtremelyGood, true),
			new CalendarDaySummary(13, null, false),
			new CalendarDaySummary(14, null, false),
			new CalendarDaySummary(15, null, false),
			new CalendarDaySummary(16, null, false),
			new CalendarDaySummary(17, null, false),
			new CalendarDaySummary(18, null, false),
			new CalendarDaySummary(19, null, false),
			new CalendarDaySummary(20, null, false),
			new CalendarDaySummary(21, null, false),
			new CalendarDaySummary(22, null, false),
			new CalendarDaySummary(23, null, false),
			new CalendarDaySummary(24, null, false),
			new CalendarDaySummary(25, null, false),
			new CalendarDaySummary(26, null, false),
			new CalendarDaySummary(27, null, false),
			new CalendarDaySummary(28, null, false),
			new CalendarDaySummary(29, null, false),
			new CalendarDaySummary(30, null, false)
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
