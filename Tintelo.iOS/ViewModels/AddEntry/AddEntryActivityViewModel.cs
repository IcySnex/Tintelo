using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.ViewModels.AddEntry;

public partial class AddActivityViewModel(
	AddEntryFlow addEntry) : ObservableObject
{
	public IReadOnlyList<Activity> Activities { get; } =
	[
		new(Guid.Empty, "Family", ActivityIcon.Family, "#F4A261", false, "a0"),
		new(Guid.Empty, "Work", ActivityIcon.Work, "#2A9D8F", false, "a1"),
		new(Guid.Empty, "Health", ActivityIcon.Health, "#E76F51", false, "a2"),
		new(Guid.Empty, "Friends", ActivityIcon.Friends, "#457B9D", false, "a3"),
		new(Guid.Empty, "Hobbies", ActivityIcon.Hobbies, "#8A7AA5", false, "a4")
	];


	[RelayCommand]
	Task CloseAsync() =>
		addEntry.CloseAsync(true);

	[RelayCommand]
	Task PreviousAsync() =>
		addEntry.PreviousAsync();

	[RelayCommand]
	Task ContinueAsync() =>
		Task.CompletedTask;
	
}
