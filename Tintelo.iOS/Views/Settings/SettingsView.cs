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
		
		Content = new CollectionView<SettingsEntry>
		{
			Layout = CollectionLayout.List(true), 
			ShowsSeparators = true,
			SeparatorInsets = new(56, 0, 12, 0),
			RetainsSelection = false,
			
			Header = new SettingsHeaderView()
			{
				TapCommand = viewModel.ShowAboutCommand
			},
			
			ItemsSource = Bind(vm => vm.Items),
			ItemTemplate = static () => new SettingsEntryCell()
		};
	}
}
