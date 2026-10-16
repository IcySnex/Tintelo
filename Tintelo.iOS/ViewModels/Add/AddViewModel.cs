using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.ViewModels.Add;

public partial class AddViewModel(
	INavigator navigator) : ObservableObject
{
	public record MoodOption(
		Mood Mood,
		bool IsSelected);


	public DateOnly Date { get; set; }

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

		OnPropertyChanged(nameof(SelectedMood));
	}

	[RelayCommand]
	Task ContinueAsync()
	{
		return navigator.PushAsync<AddViewModel>();
	}
}
