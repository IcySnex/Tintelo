using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsTimeEntryCell : SettingsEntryCell<SettingsTimeEntry>
{
	readonly DatePicker picker;

	public SettingsTimeEntryCell()
	{
		HighlightBackground = null;

		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add((picker = new DatePicker
		{
			Width = 100,
			VerticalAlignment = VerticalAlignment.Center,
			
			Mode = DatePickerMode.Time,
			Minimum = Bind(item => item.Minimum),
			Maximum = Bind(item => item.Maximum),
			
			Tint = Colors.SecondaryLabel
		}).Column(2));
	}


	protected override void OnItemChanged(
		SettingsTimeEntry item)
	{
		picker.Date = item.Date;
	}
}
