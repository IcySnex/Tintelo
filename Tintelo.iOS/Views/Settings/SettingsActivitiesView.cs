using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
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

					Icon = ImageSource.Symbol("plus", weight: FontWeight.Semibold),
					Text = Texts.Settings_Entries_Activities_Add,
				},
				new ToolbarButton
				{
					Command = Command.From(() => viewModel.IsEditing = !viewModel.IsEditing),

					Icon = Bind(vm => vm.IsEditing)
						.ConvertTo(isEditing => ImageSource.Symbol(isEditing ? "checkmark" : "pencil", weight: FontWeight.Semibold)),
					Text = Bind(vm => vm.IsEditing)
						.ConvertTo(isEditing => isEditing ? Texts.Settings_Entries_Activities_Done : Texts.Settings_Entries_Activities_Edit),
				}
			}
		});
		
		Content = new CollectionView<Activity>
		{
			Layout = CollectionLayout.List(true),
			SeparatorInsets = new(64, 0, 12, 0),
			
			IsEditing =  Bind(vm => vm.IsEditing)
				.TwoWay((vm, val) => vm.IsEditing = val),
			SelectsOnlyWhileEditing = true,

			ItemTemplate = static () => new ActivityCell(),

			ItemsSource =
			[
				new(Guid.Empty, "Family", ActivityIcon.Family, "#F4A261", false),
				new(Guid.Empty, "Work", ActivityIcon.Work, "#2A9D8F", false),
				new(Guid.Empty, "Health", ActivityIcon.Health, "#E76F51", false),
				new(Guid.Empty, "Reading", null, "#8A7AA5", false)
			]
		};
	}
}
