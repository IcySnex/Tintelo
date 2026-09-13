using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsNavigationEntryCell : SettingsEntryCell<SettingsNavigationEntry>
{
	public SettingsNavigationEntryCell()
	{
		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add(new Image
		{
			VerticalAlignment = VerticalAlignment.Center,
			Height = 13,
			Width = 8,
			
			Source = ImageSource.Symbol(
				"chevron.right",
				size: 13,
				weight: FontWeight.Semibold,
				colors: [Colors.TertiaryLabel])
		}.Column(2));
	}
}