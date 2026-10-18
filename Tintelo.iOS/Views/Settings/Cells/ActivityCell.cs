using SkeleKit;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class ActivityCell : ItemView<Activity>
{
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
					
					Child = new Image
					{
						Width = 32,
						Height = 32,
								
						Source = ImageSource.Symbol("gamecontroller.fill"),
						Tint = Bind(item => item.Color)
							.ConvertTo<Color?>(color => color.ToColor())
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
}