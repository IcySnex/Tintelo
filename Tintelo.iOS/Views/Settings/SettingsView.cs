using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.Views.Settings;

[Page]
public class SettingsView : ContentView<SettingsViewModel>
{
	public SettingsView(SettingsViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Settings_Title;
		TitleStyle = TitleStyle.Inline;

		Background = Colors.GroupedBackground;

		Content = new CollectionView<SettingsEntry, SettingsSection>
		{
			GroupedItemsSource = Bind(vm => vm.Sections),
			
			ItemTemplate = static () => new SettingsEntryCell(),
			HeaderTemplate = static () => new SettingsHeaderView(),
			
			Layout = CollectionLayout.List(true),
			ShowsSeparators = true
		};
	}
}
