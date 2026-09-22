using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Services;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels.Calendar;

public partial class CalendarViewModel(
	INavigator navigator,
	CalendarProvider calendar) : ObservableObject
{
	public CalendarProvider Calendar => calendar;
	
	public IReadOnlyList<CalendarMonthSummary> Months { get; } =
	[
		calendar.GetMonth(YearMonth.Current),
		calendar.GetMonth(YearMonth.Current.Add(1)),
		calendar.GetMonth(YearMonth.Current.Add(2)),
		calendar.GetMonth(YearMonth.Current.Add(3)),
		calendar.GetMonth(YearMonth.Current.Add(4)),
		calendar.GetMonth(YearMonth.Current.Add(5)),
		calendar.GetMonth(YearMonth.Current.Add(6)),
		calendar.GetMonth(YearMonth.Current.Add(7)),
		calendar.GetMonth(YearMonth.Current.Add(8)),
		calendar.GetMonth(YearMonth.Current.Add(9)),
		calendar.GetMonth(YearMonth.Current.Add(10)),
		calendar.GetMonth(YearMonth.Current.Add(11)),
	];
	
	
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
