using SkeleKit;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils.Extensions;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class ActivityCell : ItemView<Activity>
{
	readonly Image iconView;

	public ActivityCell()
	{
		Background = Bind(item => item.Color)
			.ConvertTo(color => new LinearGradient
			{
				Units = GradientUnits.Points,
				Start = new(0, 0),
				End = new(16 + 24 + 32, 0),
				
				Underlay = Colors.SecondaryGroupedBackground,
				Stops =
				[
					new(color.ToColor(0.15), 0),
					new(color.ToColor(0), 1)
				]
			});
		HighlightBackground = Bind(item => item.Color)
			.ConvertTo(color => new LinearGradient
			{
				Units = GradientUnits.Points,
				Start = new(0, 0),
				End = new(16 + 24 + 32, 0),
				
				Underlay = Colors.TertiaryGroupedBackground,
				Stops =
				[
					new(color.ToColor(0.15), 0),
					new(color.ToColor(0), 1)
				]
			});

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
				(iconView = new Image
				{
					Width = 24,
					Height = 24
				}),

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
		string symbol = item.Icon is ActivityIcon icon
			? icon.ToSymbol()
			: "x.circle";

		iconView.Source = ImageSource.Symbol(
			symbol,
			size: 20,
			weight: FontWeight.Semibold,
			colors: item.Color.ToColor());
		iconView.Opacity = item.Icon.HasValue ? 1 : 0.25;
	}
}
