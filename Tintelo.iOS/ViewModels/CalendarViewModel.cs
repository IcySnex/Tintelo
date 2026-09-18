using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels;

public partial class CalendarViewModel(
	INavigator navigator) : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(SelectedNavigationTitle))]
	[NotifyPropertyChangedFor(nameof(SelectedWeekday))]
	[NotifyPropertyChangedFor(nameof(SelectedDateTitle))]
	DateTime selectedDate = new(2026, 9, 13);

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasSelectedEntry))]
	[NotifyPropertyChangedFor(nameof(HasNoSelectedEntry))]
	[NotifyPropertyChangedFor(nameof(HasSelectedNote))]
	string? selectedMoodName;

	[ObservableProperty]
	Color? selectedMoodColor;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasSelectedNote))]
	bool selectedHasNote;


	public string SelectedNavigationTitle =>
		SelectedDate.ToString("MMMM d", CultureInfo.CurrentCulture);

	public string SelectedWeekday =>
		SelectedDate.ToString("dddd", CultureInfo.CurrentCulture);

	public string SelectedDateTitle =>
		SelectedDate.ToString("MMMM d, yyyy", CultureInfo.CurrentCulture);

	public bool HasSelectedEntry =>
		SelectedMoodName is not null;

	public bool HasNoSelectedEntry =>
		!HasSelectedEntry;

	public bool HasSelectedNote =>
		HasSelectedEntry && SelectedHasNote;


	internal void SelectDay(
		DateTime date,
		string? moodName,
		Color? moodColor,
		bool hasNote)
	{
		SelectedDate = date;
		SelectedMoodName = moodName;
		SelectedMoodColor = moodColor;
		SelectedHasNote = hasNote;
	}


	[RelayCommand]
	Task OpenSettingsAsync() =>
		navigator.PresentAsync<SettingsViewModel>(ModalStyle.FormSheet);
}
