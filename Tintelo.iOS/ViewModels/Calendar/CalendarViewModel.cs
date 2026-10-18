using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Services;
using Tintelo.iOS.ViewModels.AddEntry;
using Tintelo.iOS.ViewModels.Settings;
using AddEntryFlow = Tintelo.iOS.Services.AddEntryFlow;

namespace Tintelo.iOS.ViewModels.Calendar;

public partial class CalendarViewModel(
	INavigator navigator,
	ISplitView split,
	CurrentDay currentDay,
	CalendarProvider calendar,
	CalendarDetailsViewModel details,
	AddEntryFlow addEntry) : ObservableObject
{
	public CalendarProvider Calendar { get; } = calendar;

	
	[RelayCommand]
	Task OpenSettingsAsync() =>
		navigator.PresentAsync<SettingsViewModel>(ModalStyle.FormSheet);


	[RelayCommand]
	Task OpenDayAsync(
		CalendarDaySummary summary)
	{
		if (summary.Key > currentDay.Value)
			return Task.CompletedTask;

		if (summary.Mood is null)
			return addEntry.ShowAsync(summary.Key);

		details.Date = summary.Key;
		
		if (split.IsCollapsed)
			return navigator.PresentAsync<CalendarDetailsViewModel>(ModalStyle.Sheet(Detent.Content));

		split.Show(SplitViewColumn.Primary);
		return Task.CompletedTask;
	}
}
