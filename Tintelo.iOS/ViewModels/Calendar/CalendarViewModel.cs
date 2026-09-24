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
	public CalendarProvider Calendar { get; } = calendar;

	
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
