using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.ViewModels.Settings;
using Tintelo.iOS.Views.Settings.Cells;

namespace Tintelo.iOS.Views.Settings;

[Page]
public class SettingsView : ContentView<SettingsViewModel>
{
	public SettingsView(SettingsViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Settings_Title;

		Background = Colors.GroupedBackground;
		
		Content = new CollectionView<SettingsEntry, SettingsSection>
		{
			Layout = CollectionLayout.List(true), 
			ShowsSeparators = true,
			SeparatorInsets = new(56, 0, 12, 0),
			RetainsSelection = false,
			
			Header = new SettingsHeaderView()
			{
				Margin = new(0, 0, 0, 20),
				
				TapCommand = viewModel.ShowAboutCommand
			},
			
			GroupedItemsSource = Bind(vm => vm.Sections),
			ItemTemplateSelector = new ItemTemplateSelector<SettingsEntry>()
				.Add(static () => new SettingsActionEntryCell())
				.Add(static () => new SettingsDisplayEntryCell())
				.Add(static () => new SettingsNavigationEntryCell())
				.Add(static () => new SettingsToggleEntryCell())
				.Add(static () => new SettingsPickerEntryCell()),
			SectionHeaderTemplate = static () => new SettingsSectionHeaderCell(),
			ItemCommand = viewModel.ActivateCommand
		};
	}
}
