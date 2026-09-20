using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsToggleEntryCell : SettingsEntryCell<SettingsToggleEntry>
{
	readonly Switch toggle;


	public SettingsToggleEntryCell()
	{
		HighlightBackground = null;

		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add((toggle = new Switch
		{
			VerticalAlignment = VerticalAlignment.Center
		}).Column(2));
	}


	protected override void OnItemChanged(
		SettingsToggleEntry entry)
	{
		toggle.IsOn = entry.IsOn;
	}
}
