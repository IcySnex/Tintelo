using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsActivitiesViewModel : ObservableObject
{
	readonly INavigator navigator;
	readonly ActivityRepository repository;

	public SettingsActivitiesViewModel(
		INavigator navigator,
		ActivityRepository repository)
	{
		this.navigator = navigator;
		this.repository = repository;

		SelectedActivities.CollectionChanged += (_, _) =>
			OnPropertyChanged(nameof(HasSelection));
	}
	
	
	bool CanDeleteRightNow() =>
		!DeleteCommand.IsRunning && !DeleteSelectedCommand.IsRunning && !ArchiveCommand.IsRunning && !ArchiveSelectedCommand.IsRunning;
	

	public ObservableRangeCollection<Activity> Activities { get; } = [];

	public ObservableRangeCollection<Activity> SelectedActivities { get; } = [];

	public bool HasSelection => SelectedActivities.Count > 0;

    [ObservableProperty]
    public partial bool IsEditing { get; set; }


    [RelayCommand]
    void ToggleEditing() =>
	    IsEditing = !IsEditing;


    public async Task LoadAsync()
    {
	    IReadOnlyList<Activity> activities = await repository.GetAsync();
	    Activities.ReplaceAll(activities);
    }
    
    
	[RelayCommand]
	Task AddAsync() =>
		Task.CompletedTask;
	
	
	[RelayCommand]
	Task ReorderAsync(
		ItemMove<Activity> move) =>
		Task.CompletedTask;

	
	[RelayCommand(CanExecute = nameof(CanDeleteRightNow))]
	async Task DeleteSelectedAsync()
	{
		Activity[] selected = [.. SelectedActivities];
		if (selected.Length == 0 || !await navigator.ConfirmAsync(
			Texts.Question,
			Texts.Settings_Entries_Activities_Delete_Description,
			Texts.Question_Confirm_Delete,
			Texts.Question_Confirm_Keep,
			true))
			return;

		Guid[] ids = [.. selected.Select(activity => activity.Id)];
		await repository.DeleteAsync(ids);

		SelectedActivities.Clear();
		Activities.RemoveRange(selected);
	}

	[RelayCommand(CanExecute = nameof(CanDeleteRightNow))]
	async Task DeleteAsync(
		Activity delete)
	{
		if (!await navigator.ConfirmAsync(
			Texts.Question,
			Texts.Settings_Entries_Activities_Delete_Description,
			Texts.Question_Confirm_Delete,
			Texts.Question_Confirm_Keep,
			true))
			return;

		await repository.DeleteAsync(delete.Id);

		SelectedActivities.Remove(delete);
		Activities.Remove(delete);
	}
	
	
	[RelayCommand(CanExecute = nameof(CanDeleteRightNow))]
	async Task ArchiveSelectedAsync()
	{
		Activity[] selected = [.. SelectedActivities];
		if (selected.Length == 0)
			return;

		Guid[] ids = [.. selected.Select(activity => activity.Id)];
		await repository.SetArchivedAsync(ids, true);

		SelectedActivities.Clear();
		Activities.RemoveRange(selected);
	}

	[RelayCommand(CanExecute = nameof(CanDeleteRightNow))]
	async Task ArchiveAsync(
		Activity archive)
	{
		await repository.SetArchivedAsync(archive.Id, true);

		SelectedActivities.Remove(archive);
		Activities.Remove(archive);
	}
	
}