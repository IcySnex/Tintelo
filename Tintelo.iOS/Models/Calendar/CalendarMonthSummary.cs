using SkeleKit;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Models.Calendar;

public sealed class CalendarMonthSummary(
	YearMonth key,
	ObservableRangeCollection<ICalendarDaySummary> items) : ISection<ICalendarDaySummary>
{
	public YearMonth Key { get; } = key;

	public IReadOnlyList<ICalendarDaySummary> Items => items;
	
	
	public void SetItem(
		int index,
		ICalendarDaySummary item) =>
		items[index] = item;
	
	public void SetLeading(
		int count)
	{
		int current = 0;
		while (current < items.Count && items[current] is EmptyCalendarDaySummary)
			current++;

		int difference = count - current;
		switch (difference)
		{
			case 0:
				return;
			case > 0:
			{
				ICalendarDaySummary[] added = new ICalendarDaySummary[difference];
				Array.Fill(added, ICalendarDaySummary.Empty);

				items.InsertRange(0, added);
				break;
			}
			default:
				items.RemoveRange(0, -difference);
				break;
		}
	}
}
