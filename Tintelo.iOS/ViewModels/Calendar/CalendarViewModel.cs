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
	public CalendarMonthSummary[] Months = new[]
	{
		new CalendarMonthSummary(9, 2026, new ICalendarDaySummary[]
		{
			new EmptyCalendarDaySummary(),
			new EmptyCalendarDaySummary(),
			new CalendarDaySummary(1, Mood.Good, false),
			new CalendarDaySummary(2, Mood.Bad, true),
			new CalendarDaySummary(3, Mood.ExtremelyGood, false),
			new CalendarDaySummary(4, Mood.Neutral, false),
			new CalendarDaySummary(5, Mood.Neutral, false),
			new CalendarDaySummary(6, Mood.Bad, false),
			new CalendarDaySummary(7, null, false),
			new CalendarDaySummary(8, Mood.VeryGood, false),
			new CalendarDaySummary(9, Mood.VeryBad, false),
			new CalendarDaySummary(10, Mood.ExtremelyGood, false),
			new CalendarDaySummary(11, null, false),
			new CalendarDaySummary(12, null, false),
			new CalendarDaySummary(13, Mood.ExtremelyGood, false),
			new CalendarDaySummary(14, Mood.Neutral, false),
			new CalendarDaySummary(15, Mood.VeryBad, false),
			new CalendarDaySummary(16, Mood.Bad, false),
			new CalendarDaySummary(17, Mood.Neutral, false),
			new CalendarDaySummary(18, Mood.VeryGood, false),
			new CalendarDaySummary(19, null, false),
			new CalendarDaySummary(20, null, false),
			new CalendarDaySummary(21, null, false),
			new CalendarDaySummary(22, null, false),
			new CalendarDaySummary(23, null, false),
			new CalendarDaySummary(24, Mood.ExtremelyGood, false),
			new CalendarDaySummary(25, Mood.Neutral, false),
			new CalendarDaySummary(26, Mood.VeryGood, false),
			new CalendarDaySummary(27, Mood.ExtremelyGood, false),
			new CalendarDaySummary(28, Mood.ExtremelyGood, false),
			new CalendarDaySummary(29, Mood.Neutral, false),
			new CalendarDaySummary(30, Mood.VeryGood, false),
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
