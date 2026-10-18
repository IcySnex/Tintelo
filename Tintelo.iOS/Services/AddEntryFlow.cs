using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.AddEntry;
using Tintelo.iOS.ViewModels.AddEntry;

namespace Tintelo.iOS.Services;

public partial class AddEntryFlow(
	INavigator navigator,
	CurrentDay currentDay) : ObservableObject
{
	public EntryDraft? Draft { get; private set; }


	public Task ShowAsync(
		DateOnly date)
	{
		Draft = new(date);
		
		AddMoodViewModel vm = new(this);
		return navigator.PresentAsync(vm, ModalStyle.Sheet(Detent.Content));
	}

	public async Task CloseAsync(
		bool showWarning)
	{
		if (showWarning && !await navigator.ConfirmAsync(
			Texts.Question,
			Texts.Question_Description_DataDiscard,
			Texts.Question_Confirm_Discard,
			Texts.Question_Confirm_Stay,
			true))
			return;
		
		await navigator.DismissAsync();
	}

	public Task PreviousAsync() =>
		navigator.PopAsync();
	
	public Task ContinueWithActivitiesAsync()
	{
		if (Draft is null)
			throw new InvalidOperationException("Start the add flow before pushing its steps.");
		
		AddActivityViewModel vm = new(this);
		return navigator.PushAsync(vm);
	}


	[RelayCommand]
	Task ShowCurrentDayAsync() =>
		ShowAsync(currentDay.Value);
}
