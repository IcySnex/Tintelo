using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.ViewModels.Settings;
using Tintelo.iOS.Views.Settings.Cells;

namespace Tintelo.iOS.Views.Settings;

[Page]
public class SettingsActivitiesView : ContentView<SettingsActivitiesViewModel>
{
	public SettingsActivitiesView(SettingsActivitiesViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Settings_Entries_Activities;
		Background = Colors.GroupedBackground;

		ToolbarItems.Add(new ToolbarGroup
		{
			Items =
			{
				new ToolbarButton
				{
					IsVisible = Bind(vm => vm.IsEditing)
						.ConvertTo(isEditing => !isEditing),

					Command = viewModel.AddCommand,

					Text = Texts.Settings_Entries_Activities_Add,
					Icon = ImageSource.Symbol("plus")
				},
				new ToolbarButton
				{
					Command = Command.From(() => viewModel.IsEditing = !viewModel.IsEditing),

					Text = Bind(vm => vm.IsEditing)
						.ConvertTo(isEditing => isEditing
							? Texts.Settings_Entries_Activities_Done
							: Texts.Settings_Entries_Activities_Edit),
					Icon = Bind(vm => vm.IsEditing)
						.ConvertTo(isEditing => ImageSource.Symbol(isEditing
							? "checkmark"
							: "pencil")),
					IsPrimary = Bind(vm => vm.IsEditing)
				}
			}
		});
		ToolbarItems.Add(new ToolbarGroup
		{
			IsVisible = Bind(vm => vm.IsEditing),
			Items =
			{
				new ToolbarButton
				{
					Command = viewModel.ToggleArchiveSelectedCommand,

					Text = Bind(vm => vm.SelectionIsArchived)
						.ConvertTo(isArchived => isArchived
							? Texts.Settings_Entries_Activities_Unarchive
							: Texts.Settings_Entries_Activities_Archive),
					Icon = ImageSource.Symbol("archivebox")
				},
				new ToolbarButton
				{
					IsEnabled = Bind(vm => vm.HasSelection),
			
					Command = viewModel.DeleteSelectedCommand,

					Text = Texts.Settings_Entries_Activities_Delete,
					Icon = ImageSource.Symbol("trash")
				}
			}
		});
		
		Content = new CollectionView<Activity, ActivitySection>
		{
			Layout = CollectionLayout.List(true),
			SeparatorInsets = new(64, 0, 12, 0),
			SelectsOnlyWhileEditing = true,
			ReordersOnlyWhileEditing = true,
			
			IsEditing =  Bind(vm => vm.IsEditing)
				.TwoWay((vm, val) => vm.IsEditing = val),
			
			EmptyView = new StackPanel
			{
				SystemInsetEdges = LayoutEdges.All,
				VerticalAlignment = VerticalAlignment.Center,
				
				Spacing = 12,
				
				Children =
				{
					new Label
					{
						Text = Texts.Settings_Entries_Activities_Empty,
						TextColor = Colors.SecondaryLabel,
						TextAlignment = TextAlignment.Center,
					},
					
					new Button
					{
						HorizontalAlignment = HorizontalAlignment.Center,
						
						Command = viewModel.AddCommand,
						
						Text = Texts.Settings_Entries_Activities_Empty_CallToAction,
						Icon = ImageSource.Symbol("plus"),
						Kind = ButtonStyle.Tinted,
						Size = ButtonSize.Small,
					}
				}
			},
			
			ItemTemplate = static () => new ActivityCell(),
			SectionHeaderTemplate = static () => new ActivitySectionHeaderCell(),

			GroupedItemsSource = viewModel.Sections,
			SelectedItems = viewModel.SelectedActivities,
			ReorderCommand = viewModel.ReorderCommand,
			
			SwipeActions =
			{
				new SwipeAction
				{
					Command = viewModel.DeleteCommand,
					
					Icon = ImageSource.Symbol("trash"),
					IsDestructive = true
				},
				new SwipeAction
				{
					Command = viewModel.ToggleArchiveCommand,
					
					Icon = ImageSource.Symbol("archivebox")
				}
			}
		};
	}


	protected override void OnLoaded()
	{
		_ = ViewModel.LoadAsync();
	}
	
	protected override void OnDisappeared()
	{
		ViewModel.IsEditing = false;
		ViewModel.SelectedActivities.Clear();
	}
}
