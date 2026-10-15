using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.ViewModels.Add;

public partial class AddViewModel(
	INavigator navigator,
	CurrentDay currentDay) : ObservableObject
{
	public record MoodOption(
		Mood Mood,
		bool IsSelected);

	
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial DateOnly? Date { get; set; }

	[ObservableProperty]
	public partial Mood? SelectedMood { get; private set; }


	public ObservableRangeCollection<MoodOption> Moods { get; } = new()
	{
		new(Mood.ExtremelyBad, false),
		new(Mood.VeryBad, false),
		new(Mood.Bad, false),
		new(Mood.Neutral, false),
		new(Mood.Good, false),
		new(Mood.VeryGood, false),
		new(Mood.ExtremelyGood, false)
	};
	
	public string Title =>
		Date is DateOnly day
			? day.ToString(Texts.DateTime_Format_yMMMMd, CultureInfo.CurrentCulture)
			: Texts.AddEntry;
	

	[RelayCommand]
	Task ShowAsync()
	{
		Date = currentDay.Value;

		return navigator.PresentAsync<AddViewModel>(ModalStyle.Sheet(Detent.Content));
	}


	[RelayCommand]
	void SelectMood(
		MoodOption option)
	{
		for (int index = 0; index < Moods.Count; index++)
		{
			MoodOption current = Moods[index];
			bool selected = current.Mood == option.Mood;

			if (current.IsSelected != selected)
				Moods[index] = current with { IsSelected = selected };
		}

		SelectedMood = option.Mood;
	}

	[RelayCommand]
	Task ContinueAsync()
	{
		return navigator.PushAsync<AddViewModel>();
	}
}
