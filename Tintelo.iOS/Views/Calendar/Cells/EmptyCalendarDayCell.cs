using SkeleKit;
using Tintelo.iOS.Models.Calendar;

namespace Tintelo.iOS.Views.Calendar.Cells;

public class EmptyCalendarDayCell : ItemView<EmptyCalendarDaySummary>
{
	public EmptyCalendarDayCell()
	{
		MaxHeight = 60;
		MaxWidth = 60;
		
		IsEnabled = false;
		
		HighlightBackground = null;
	}
}