using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings;

public class SettingsEntryCell : ItemView<SettingsEntry>
{
	public SettingsEntryCell()
	{
		Background = Colors.SecondaryGroupedBackground;
		
		Content = new Grid
		{
			Margin = new(16, 0),
			
			ColumnSpacing = 16,
			Columns =
			{
				24,
				GridLength.Star
			},
			
			Children =
			{
				new Image
				{
					VerticalAlignment = VerticalAlignment.Center,
					Height = 24,
					Width = 24,
					Source = Bind(entry => entry.Icon)
						.ConvertTo(icon => ImageSource.Symbol(
							icon,
							scale: SymbolScale.Small,
							colors: [Colors.Label, Colors.Label, Colors.Label])),
				},
				
				new Label
				{
					VerticalAlignment = VerticalAlignment.Center,

					Text = Bind(entry => entry.Title),
					TextStyle = TextStyle.Body,
					MaxLines = 1
				}.Column(1)
			}
		};
	}
}
