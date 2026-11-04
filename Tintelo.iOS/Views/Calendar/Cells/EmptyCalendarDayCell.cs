using SkeleKit;
using Tintelo.iOS.Models.Calendar;

namespace Tintelo.iOS.Views.Calendar.Cells;

public class EmptyCalendarDayCell : ItemView<EmptyCalendarDaySummary>
{
	public EmptyCalendarDayCell()
	{
		MaxHeight = CalendarView.CellWideRowHeight;
		MaxWidth = CalendarView.CellWideRowHeight;
		
		IsEnabled = false;
		
		HighlightBackground = null;
	}

	
	protected override Size MeasureOverride(
		Size availableSize) =>
		new(0, CalendarView.CellRowHeight(availableSize.Width));
}