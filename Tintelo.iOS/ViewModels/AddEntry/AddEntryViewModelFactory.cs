using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.AddEntry;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.ViewModels.AddEntry;

public partial class AddEntryViewModelFactory(
	INavigator navigator,
	CurrentDay currentDay) : ObservableObject
{
	EntryDraft? draft;


	public Task ShowAsync(
		DateOnly date)
	{
		draft = new(date);
		
		AddMoodViewModel vm = new(navigator, this, draft);
		return navigator.PresentAsync(vm, ModalStyle.Sheet(Detent.Content));
	}

	public Task ContinueWithCategoriesAsync()
	{
		if (draft is null)
			throw new InvalidOperationException("Start the add flow before pushing its steps.");
		
		AddCategoryViewModel vm = new(navigator, draft);
		return navigator.PushAsync(vm);
	}


	[RelayCommand]
	Task ShowCurrentDayAsync() =>
		ShowAsync(currentDay.Value);
}
