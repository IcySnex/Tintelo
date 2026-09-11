using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings;

public class SettingsEntryCell : ItemView<SettingsEntry>
{
	public SettingsEntryCell() =>
		Content = new Border
		{
			Height = 52,

			Child = new Label
			{
				Margin = new(16, 0),
				VerticalAlignment = VerticalAlignment.Center,
				
				Text = Bind(entry => entry.Title),
				TextStyle = TextStyle.Body
			}
		};
}
