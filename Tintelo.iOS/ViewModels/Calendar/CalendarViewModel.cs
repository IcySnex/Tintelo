using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Services;
using Tintelo.iOS.ViewModels.Add;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels.Calendar;

public partial class CalendarViewModel(
	INavigator navigator,
	ISplitView split,
	CurrentDay currentDay,
	CalendarProvider calendar,
	CalendarDetailsViewModel details,
	AddViewModel add) : ObservableObject
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
		{
			add.Date = summary.Key;

			return navigator.PresentAsync<AddViewModel>(ModalStyle.Sheet(Detent.Content));
		}

		details.Date = summary.Key;
		if (split.IsCollapsed)
			return navigator.PresentAsync<CalendarDetailsViewModel>(ModalStyle.Sheet(Detent.Content));

		split.Show(SplitViewColumn.Primary);

		return Task.CompletedTask;
	}
}
