using System.Globalization;
using SkeleKit;

namespace Tintelo.iOS.Views.Calendar;

public class WeekdaysHeader : Border
{
	public WeekdaysHeader(
		DayOfWeek firstWeekday)
	{
		string[] weekdays = Enumerable.Range(0, 7)
			.Select(offset => CultureInfo.CurrentCulture.DateTimeFormat.ShortestDayNames[((int)firstWeekday + offset) % 7].ToUpper(CultureInfo.CurrentCulture))
			.ToArray();
		
		
		Grid container = new()
		{
			Margin = new(16, 8),
			ColumnSpacing = 6
		};

		for (int column = 0; column < 7; column++)
		{
			container.Columns.Add(GridLength.Star);
			
			container.Children.Add(new Label
			{
				Text = weekdays[column],
				TextStyle = TextStyle.Caption1,
				
				TextAlignment = TextAlignment.Center,
				FontWeight = FontWeight.Semibold,
				TextColor = Colors.SecondaryLabel,
				
				MaxLines = 1,
				MaxFontSize = 19,
				AutoShrink = 0.7
			}.Column(column));
		}

		Child = container;
	}
}