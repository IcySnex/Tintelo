using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsDisplayEntryCell : SettingsEntryCell<SettingsDisplayEntry>
{
	readonly LabelAccessory labelAccessory;
	
	public SettingsDisplayEntryCell()
	{
		HighlightBackground = null;
		
		Accessories.Add(labelAccessory = new LabelAccessory());
	}


	protected override void OnItemChanged(
		SettingsDisplayEntry item)
	{
		labelAccessory.Text = item.Text;
	}
}