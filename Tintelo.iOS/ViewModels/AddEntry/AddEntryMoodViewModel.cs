using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.AddEntry;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.ViewModels.AddEntry;

public partial class AddMoodViewModel(
	INavigator navigator,
	AddEntryViewModelFactory addEntry,
	EntryDraft draft) : ObservableObject
{
	public DateOnly Date => draft.Date;
	
	public string Title => Date.ToString(Texts.DateTime_Format_yMMMMd, CultureInfo.CurrentCulture);

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

	public Mood? SelectedMood => Moods.FirstOrDefault(o => o.IsSelected)?.Mood;


	[RelayCommand]
	async Task CloseAsync()
	{
		if (SelectedMood.HasValue && !await navigator.ConfirmAsync(
			Texts.AddEntry_ConfirmLeave,
			Texts.AddEntry_ConfirmLeave_Description,
			Texts.AddEntry_ConfirmLeave_Yes,
			Texts.AddEntry_ConfirmLeave_No,
			true))
			return;
		
		await navigator.DismissAsync();
	}

	[RelayCommand]
	Task ContinueAsync() =>
		addEntry.ContinueWithActivitiesAsync();
	
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

		draft.Mood = option.Mood;
		OnPropertyChanged(nameof(SelectedMood));
	}
}
