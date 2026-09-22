using System.Globalization;
using SkeleKit;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Views.Calendar;

public class WeekdaysHeader : Border
{
	public WeekdaysHeader(
		CalendarProvider calendar)
	{
		Grid container = new()
		{
			Margin = new(16, 8),
			ColumnSpacing = 6
		};

		for (int column = 0; column < 7; column++)
		{
			int offset = column;
			
			container.Columns.Add(GridLength.Star);
			container.Children.Add(new Label
			{
				Text = BindingFactory.Bind(calendar, calendar => calendar.FirstWeekday)
					.ConvertTo(value => CultureInfo.CurrentCulture.DateTimeFormat.ShortestDayNames[((int)value + offset) % 7].ToUpper(CultureInfo.CurrentCulture)),
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