using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Models.Settings;
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

		ActiveSection = new([]);
		ArchivedSection = new([]);
		Sections = [ActiveSection];

		ArchivedSection.Items.CollectionChanged += OnArchivedItemsChanged;
		SelectedActivities.CollectionChanged += OnSelectedActivitiesChanged;
	}

	
	void OnArchivedItemsChanged(
		object? _,
		NotifyCollectionChangedEventArgs e)
	{
		bool hasItems = ArchivedSection.Items.Count > 0;
		if (hasItems == Sections.Contains(ArchivedSection))
			return;

		if (hasItems)
		{
			Sections.Add(ArchivedSection);
		}
		else
		{
			Sections.Remove(ArchivedSection);
			ArchivedSection.IsExpanded = false;
		}
	}
	
	void OnSelectedActivitiesChanged(
		object? _,
		NotifyCollectionChangedEventArgs e)
	{
		OnPropertyChanged(nameof(HasSelection));
		OnPropertyChanged(nameof(SelectionIsArchived));
		ToggleArchiveSelectedCommand.NotifyCanExecuteChanged();
	}

	
	public ObservableRangeCollection<ActivitySection> Sections { get; }
	public ActivitySection ActiveSection { get; }
	public ActivityArchiveSection ArchivedSection { get; }

	public ObservableRangeCollection<Activity> SelectedActivities { get; } = [];
	public bool HasSelection => SelectedActivities.Count > 0;
	public bool SelectionIsArchived => SelectedActivities.Count > 0 && SelectedActivities.All(activity => activity.IsArchived);
	
	[ObservableProperty]
	public partial bool IsEditing { get; set; }
	

	public async Task LoadAsync()
	{
		IReadOnlyList<Activity> activities = await repository.GetAsync(includeArchived: true);
		ActiveSection.Items.ReplaceAll(activities.Where(activity => !activity.IsArchived));
		ArchivedSection.Items.ReplaceAll(activities.Where(activity => activity.IsArchived));
	}


	// Commands
	bool CanRunActions() =>
		!DeleteCommand.IsRunning
		&& !DeleteSelectedCommand.IsRunning
		&& !ToggleArchiveCommand.IsRunning
		&& !ToggleArchiveSelectedCommand.IsRunning;

	bool CanToggleArchiveSelection() =>
		SelectedActivities.Count > 0
		&& (SelectedActivities.All(activity => activity.IsArchived) || SelectedActivities.All(activity => !activity.IsArchived))
		&& CanRunActions();

	
	[RelayCommand]
	Task AddAsync() =>
		Task.CompletedTask;


	[RelayCommand]
	async Task ReorderAsync(
		ItemMove<Activity> move)
	{
		ActivitySection destination = move.ToSection == 0
			? ActiveSection
			: ArchivedSection;

		Guid? previousId = move.ToIndex > 0
			? destination.Items[move.ToIndex - 1].Id
			: null;

		Guid? nextId = move.ToIndex < destination.Items.Count - 1
			? destination.Items[move.ToIndex + 1].Id
			: null;

		bool isArchived = destination is ActivityArchiveSection;

		await repository.MoveAsync(
			move.Item.Id,
			previousId,
			nextId,
			isArchived);

		if (move.FromSection == move.ToSection)
			return;

		int index = destination.Items.IndexOf(move.Item);
		if (index >= 0)
			destination.Items[index] = move.Item with { IsArchived = isArchived };

		SelectedActivities.Clear();
	}


	[RelayCommand(CanExecute = nameof(CanRunActions))]
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

		Guid[] ids = selected
			.Select(activity => activity.Id)
			.ToArray();
		await repository.DeleteAsync(ids);

		SelectedActivities.Clear();
		await LoadAsync();
	}

	[RelayCommand(CanExecute = nameof(CanRunActions))]
	async Task DeleteAsync(
		Activity activity)
	{
		if (!await navigator.ConfirmAsync(
			Texts.Question,
			Texts.Settings_Entries_Activities_Delete_Description,
			Texts.Question_Confirm_Delete,
			Texts.Question_Confirm_Keep,
			true))
			return;

		await repository.DeleteAsync(activity.Id);

		SelectedActivities.Clear();
		await LoadAsync();
	}


	[RelayCommand(CanExecute = nameof(CanToggleArchiveSelection))]
	async Task ToggleArchiveSelectedAsync()
	{
		Activity[] selected = [.. SelectedActivities];
		if (selected.Length == 0)
			return;

		Guid[] archiveIds = selected
			.Where(activity => !activity.IsArchived)
			.Select(activity => activity.Id)
			.ToArray();
		if (archiveIds.Length > 0)
			await repository.SetArchivedAsync(archiveIds, true);
		
		Guid[] unarchiveIds = selected
			.Where(activity => activity.IsArchived)
			.Select(activity => activity.Id)
			.ToArray();
		if (unarchiveIds.Length > 0)
			await repository.SetArchivedAsync(unarchiveIds, false);

		SelectedActivities.Clear();
		await LoadAsync();
	}
	
	[RelayCommand(CanExecute = nameof(CanRunActions))]
	async Task ToggleArchiveAsync(
		Activity activity)
	{
		await repository.SetArchivedAsync(activity.Id, !activity.IsArchived);

		SelectedActivities.Clear();
		await LoadAsync();
	}
}
