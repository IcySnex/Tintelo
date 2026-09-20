using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsPickerEntryCell : SettingsEntryCell<SettingsPickerEntry>
{
	readonly Picker<string> picker;
	
	public SettingsPickerEntryCell()
	{
		HighlightBackground = null;

		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add((picker = new Picker<string>
		{
			VerticalAlignment = VerticalAlignment.Center,
			Padding = OperatingSystem.IsIOSVersionAtLeast(26) ? null : Thickness.Zero,
			
			Tint = Bind(item => item.Color),
			Kind = OperatingSystem.IsIOSVersionAtLeast(26) ? ButtonStyle.Tinted : ButtonStyle.Plain,
			
			ItemsSource = Bind(item => item.Options)
		}).Column(2));
	}


	protected override void OnItemChanged(
		SettingsPickerEntry item)
	{
		picker.SelectedItem = item.SelectedOption;
	}
}
