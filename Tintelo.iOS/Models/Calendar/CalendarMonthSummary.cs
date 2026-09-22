using SkeleKit;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Models.Calendar;

public sealed class CalendarMonthSummary(
	YearMonth key,
	ObservableRangeCollection<ICalendarDaySummary> items)
	: ISection<ICalendarDaySummary>
{
	public YearMonth Key { get; } = key;

	public IReadOnlyList<ICalendarDaySummary> Items => items;
	
	
	public void SetItem(
		int index,
		ICalendarDaySummary item) =>
		items[index] = item;
	
	public void SetLeading(
		int blanks)
	{
		int current = 0;
		while (current < items.Count && items[current] is EmptyCalendarDaySummary)
			current++;
		
		if (blanks == current)
			return;
		
		if (blanks > current)
			items.InsertRange(0, Enumerable.Range(0, blanks - current).Select(_ => new EmptyCalendarDaySummary()));
		else
			items.RemoveRange(0, current - blanks);
	}
}
