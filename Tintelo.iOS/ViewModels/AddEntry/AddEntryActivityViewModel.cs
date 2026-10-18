using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.AddEntry;
using Tintelo.iOS.Models.Journal;

namespace Tintelo.iOS.ViewModels.AddEntry;

public partial class AddActivityViewModel(
	INavigator navigator,
	EntryDraft draft) : ObservableObject
{
	public IReadOnlyList<Activity> Activities { get; } =
	[
		new(Guid.Empty, "Family", ActivityIcon.Family, "#F4A261", false),
		new(Guid.Empty, "Work", ActivityIcon.Work, "#2A9D8F", false),
		new(Guid.Empty, "Health", ActivityIcon.Health, "#E76F51", false),
		new(Guid.Empty, "Friends", ActivityIcon.Friends, "#457B9D", false),
		new(Guid.Empty, "Hobbies", ActivityIcon.Hobbies, "#8A7AA5", false)
	];
	
	
	[RelayCommand]
	async Task CloseAsync()
	{
		if (!await navigator.ConfirmAsync(
			Texts.AddEntry_ConfirmLeave,
			Texts.AddEntry_ConfirmLeave_Description,
			Texts.AddEntry_ConfirmLeave_Yes,
			Texts.AddEntry_ConfirmLeave_No,
			true))
			return;
		
		await navigator.DismissAsync();
	}

	[RelayCommand]
	Task PreviousAsync() =>
		navigator.PopAsync();

	[RelayCommand]
	Task ContinueAsync() =>
		Task.CompletedTask;
	
}
