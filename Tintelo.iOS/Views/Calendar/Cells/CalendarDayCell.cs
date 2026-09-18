using System.Globalization;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;

namespace Tintelo.iOS.Views.Calendar.Cells;

public class CalendarDayCell: ItemView<ICalendarDaySummary>
{
	const double MaxVisualSize = 52;
	
	static readonly Color EmptyBackgroundColor = Color.Dynamic(Color.FromHex(0xf5f5f5), Color.FromHex(0x262626));
	static readonly Color EmptyStrokeColor = Color.Dynamic(Color.FromHex(0xdbdbdb), Color.FromHex(0x636363));
	static readonly Color EmptyTextColor = Color.Dynamic(Color.FromHex(0x646464), Color.FromHex(0xdadada));

	
	readonly Border contentBorder;
	readonly Label numberLabel;
	readonly Border dotBorder;
	
	public CalendarDayCell()
	{
		MaxHeight = 60;
		MaxWidth = 60;
		HighlightBackground = null;

		Content = contentBorder = new Border
		{
			MaxWidth = MaxVisualSize,
			MaxHeight = MaxVisualSize,
			Margin = 2,
			
			CornerRadius = MaxVisualSize / 2,
			StrokeLineCap = StrokeLineCap.Round,

			Child = new Overlay
			{
				Children =
				{
					(numberLabel = new Label
					{
						HorizontalAlignment = HorizontalAlignment.Stretch,

						MaxLines = 1,

						TextStyle = TextStyle.Callout,
						FontWeight = FontWeight.Semibold,
						TextAlignment = TextAlignment.Center,

						MaxFontSize = 24,
						AutoShrink = 0.7
					}),

					(dotBorder = new Border
					{
						Width = 4,
						Height = 4,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.End,
						Margin = new Thickness(0, 0, 0, 6),
					
						CornerRadius = 2
					})
				}
			}
		};
	}
	
	
	protected override void OnItemChanged(
		ICalendarDaySummary? item)
	{
		if (item is not CalendarDaySummary summary)
		{
			contentBorder.IsVisible = false;
			return;
		}
		
		bool isFuture = summary.Day > DateTime.Now.Day;
		bool isToday = summary.Day == DateTime.Now.Day;
		bool isEmpty = !isFuture && summary.Mood is null && !isToday;
		
		contentBorder.IsVisible = true;
		contentBorder.Background = isFuture ? Colors.Transparent : isEmpty ? EmptyBackgroundColor : Colors.Blue; // RESOLVE FROM PALETTE BASED ON MOOD
		contentBorder.Stroke = isToday ? Colors.Label.WithAlpha(0.72) : isEmpty ? EmptyStrokeColor : null;
		contentBorder.StrokeThickness = isToday || isEmpty ? 2 : 0;
		contentBorder.StrokeDashPattern = isEmpty ? [1, 5] : null;
		
		numberLabel.Text = summary.Day.ToString(CultureInfo.CurrentCulture);
		numberLabel.TextColor = isFuture ? Colors.SecondaryLabel.WithAlpha(0.6) : isEmpty ? EmptyTextColor : Colors.Red; // RESOLVE FROM PALETTE BASED ON MOOD

		dotBorder.IsVisible = summary.HasNote;
		dotBorder.Background = Colors.Red; // RESOLVE FROM PALETTE BASED ON MOOD // future cant have content so we only need mood based palette color
	}
}
