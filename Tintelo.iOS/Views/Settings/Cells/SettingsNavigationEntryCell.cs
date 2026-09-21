using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsNavigationEntryCell : SettingsEntryCell<SettingsNavigationEntry>
{
	readonly LabelAccessory labelAccessory;

	public SettingsNavigationEntryCell()
	{
		Accessories.Add(labelAccessory = new LabelAccessory());
		Accessories.Add(new DisclosureAccessory());
	}
	
	
	protected override void OnItemChanged(
		SettingsNavigationEntry item)
	{
		labelAccessory.Text = item.Text ?? default;
	}
}