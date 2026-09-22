using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels.Calendar;

public partial class CalendarViewModel : ObservableObject
{
	readonly INavigator navigator;

	public CalendarViewModel(INavigator navigator,
		CalendarProvider calendar)
	{
		this.navigator = navigator;
		this.Calendar = calendar;
		
		Months =
		[
			..CreateTestMonths(1200)
		];
	}

	
	IEnumerable<CalendarMonthSummary> CreateTestMonths(
		int count)
	{
		for (int i = 0; i < count; i++)
		{
			yield return Calendar.GetMonth(YearMonth.Current.Add(i));
		}
	}
	
	
	public CalendarProvider Calendar { get; }

	public IReadOnlyList<CalendarMonthSummary> Months { get; }
	
	
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
