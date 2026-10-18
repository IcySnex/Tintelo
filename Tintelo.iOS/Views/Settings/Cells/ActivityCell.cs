using SkeleKit;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class ActivityCell : ItemView<Activity>
{
	readonly Image icon;

	public ActivityCell()
	{
		Background = Colors.SecondaryGroupedBackground;

		Content = new Grid
		{
			Margin = new(16, 0),
			
			ColumnSpacing = 16,
			Columns =
			{
				32,
				GridLength.Star
			},
			
			Children =
			{
				new Border
				{
					Margin = new(-16, 0, -32, 0),
					Padding = new(16, 0, 32, 0),
					
					Background = Bind(item => item.Color)
						.ConvertTo(color => new LinearGradient
						{
							Start = new(0, 0.5),
							End = new(1, 0.5),
							Stops =
							[
								new(color.ToColor(0.15), 0),
								new(color.ToColor(0.0), 1)
							]
						}),
					
					Child = icon = new Image
					{
						Width = 32,
						Height = 32
					}
				},
				
				new Label
				{
					Text = Bind(activity => activity.Name),
					MaxLines = 1
				}.Column(1)
			}
		};
	}


	protected override void OnItemChanged(
		Activity item)
	{
		icon.Source = ImageSource.Symbol(
			item.Icon?.ToSymbol() ?? "x.circle",
			size: 20,
			weight: FontWeight.Semibold,
			colors: item.Color.ToColor(item.Icon.HasValue ? 1 : 0.5));
	}
}
