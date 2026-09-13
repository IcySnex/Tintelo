using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsDisplayEntryCell : SettingsEntryCell<SettingsDisplayEntry>
{
	public SettingsDisplayEntryCell()
	{
		HighlightBackground = null;

		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add(new Label
		{
			VerticalAlignment = VerticalAlignment.Center,
			
			TextColor = Colors.SecondaryLabel,
			
			Text = Bind(entry => entry.Value),
			MaxLines = 1
		}.Column(2));
	}
}