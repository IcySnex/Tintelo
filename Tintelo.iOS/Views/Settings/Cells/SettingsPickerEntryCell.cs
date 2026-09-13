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
			Padding = Thickness.Zero,
			
			Tint = Colors.SecondaryLabel,
			Kind = ButtonStyle.Plain,
			
			ItemsSource = Bind(item => item.Options)
		}).Column(2));
	}


	protected override void OnItemChanged(
		SettingsPickerEntry? item)
	{
		picker.SelectedItem = item?.SelectedOption ?? default;
	}
}
