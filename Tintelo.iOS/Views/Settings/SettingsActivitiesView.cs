using System.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.Views.Settings;

[Page]
public class SettingsActivitiesView : ContentView<SettingsActivitiesViewModel>
{
	readonly ToolbarButton archiveButton;
	readonly ToolbarButton editButton;
	readonly ToolbarButton addButton;

	public SettingsActivitiesView(SettingsActivitiesViewModel viewModel) : base(viewModel)
	{
		viewModel.PropertyChanged += OnViewModelPropertyChanged;

		Title = Texts.Settings_Entries_Activities;
		
		ToolbarItems.Add(new ToolbarGroup
		{
			Items =
			{
				(addButton = new ToolbarButton
				{
					Command = viewModel.AddCommand,

					Icon = ImageSource.Symbol("plus", weight: FontWeight.Semibold),
					Text = Texts.Settings_Entries_Activities_Add,
				}),
				(editButton = new ToolbarButton
				{
					Command = Command.From(() => viewModel.IsEditing = !viewModel.IsEditing),

					Icon = ImageSource.Symbol("pencil", weight: FontWeight.Semibold),
					Text = Texts.Settings_Entries_Activities_Edit,
				})
			}
		});
		ToolbarItems.Add(archiveButton = new ToolbarButton
		{
			Command = viewModel.ShowArchiveCommand,

			Icon = ImageSource.Symbol("archivebox", weight: FontWeight.Semibold),
			Text = Texts.Settings_Entries_Activities_Archive,
		});
	}

	
	void OnViewModelPropertyChanged(object? _, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(SettingsActivitiesViewModel.IsEditing):
				bool isEditing = ViewModel.IsEditing;

				editButton.Icon = ImageSource.Symbol(
					isEditing ? "checkmark" : "pencil",
					weight: FontWeight.Semibold);
				editButton.Text = isEditing
					? Texts.Settings_Entries_Activities_Done
					: Texts.Settings_Entries_Activities_Edit;
				archiveButton.IsVisible = !isEditing;
				addButton.IsVisible = !isEditing;
				break;
		}
	}
}
