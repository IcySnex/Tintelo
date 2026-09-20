using SkeleKit;
using Tintelo.iOS.Models.Calendar;

namespace Tintelo.iOS.Views.Calendar.Cells;

public class EmptyCalendarDayCell : ItemView<EmptyCalendarDaySummary>
{
	public EmptyCalendarDayCell()
	{
		IsEnabled = false;
		
		HighlightBackground = null;
	}
}