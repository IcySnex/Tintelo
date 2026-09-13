using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public abstract class SettingsEntryCell<TEntry> : ItemView<TEntry>
	where TEntry : SettingsEntry
{
	protected Grid ContainerView { get; }
	protected Image IconView { get; }
	protected Label TextView { get; }
	
	protected SettingsEntryCell()
	{
		Background = Colors.SecondaryGroupedBackground;

		Content = ContainerView = new()
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
				(IconView = new()
				{
					VerticalAlignment = VerticalAlignment.Center,
					Height = 24,
					Width = 24,
					
					Tint = Colors.Label,
					
					Source = Bind(entry => entry.Icon)
						.ConvertTo(icon => ImageSource.Symbol(icon, scale: SymbolScale.Small))
				}),
				(TextView = new()
				{
					VerticalAlignment = VerticalAlignment.Center,
					
					Text = Bind(entry => entry.Title),
					MaxLines = 1
				}).Column(1)
			}
		};
	}
}
