using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

[Page]
public class SettingsMoodPaletteView : ContentView<SettingsMoodPaletteViewModel>
{
	public SettingsMoodPaletteView(SettingsMoodPaletteViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Settings_Theme_MoodPalette;
		Background = Colors.GroupedBackground;

		Content = new CollectionView<SettingsMoodPaletteViewModel.MoodPaletteContainer>
		{
			Layout = CollectionLayout.List(true),
			ShowsSelectionCheckmark = true,
			
			ItemTemplate = static () => new SettingsMoodPaletteContainerCell(),

			ItemsSource = viewModel.Palettes,
			SelectedItem = Bind(vm => vm.SelectedPalette)
				.TwoWay((vm, val) => vm.SelectedPalette = val)
		};
	}
}