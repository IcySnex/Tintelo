using System.Globalization;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;

namespace Tintelo.iOS.Views.Calendar.Cells;

public class CalendarMonthHeaderCell : ItemView<CalendarMonthSummary>
{
	public CalendarMonthHeaderCell()
	{
		HighlightBackground = null;

		Content = new StackPanel
		{
			Margin = new(0, 12, 0, 4),
			Orientation = Orientation.Horizontal,
			Spacing = 7,

			Children =
			{
				new Label
				{
					Text = Bind(month => month.Month)
						.ConvertTo(value => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value)),
					
					TextStyle = TextStyle.Title2,
					FontWeight = FontWeight.Semibold,
					MaxLines = 1
				},
				new Label
				{
					VerticalAlignment = VerticalAlignment.End,
					
					Text = Bind(month => month.Year)
						.ConvertTo(value => value.ToString(CultureInfo.CurrentCulture)),
					TextColor = Colors.SecondaryLabel,
					
					TextStyle = TextStyle.Subheadline,
					FontWeight = FontWeight.Semibold,
					MaxLines = 1
				}
			}
		};
	}
}
	