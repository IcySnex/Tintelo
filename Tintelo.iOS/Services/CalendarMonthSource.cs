using System.Collections;
using SkeleKit;
using Tintelo.iOS.Models.Calendar;

namespace Tintelo.iOS.Services;

public sealed class CalendarMonthSource(
	CalendarProvider calendar) : IVirtualizedSectionList<ICalendarDaySummary, CalendarMonthSummary>
{
	readonly YearMonth lastMonth = YearMonth.Current;

	
	public int Count => checked((lastMonth.Year - 1) * 12 + lastMonth.Month);

	public CalendarMonthSummary this[int index] => calendar.GetMonth(MonthAt(index));


	public int GetItemCount(
		int section) =>
		CalendarProvider.GetMonthItemCount(MonthAt(section), calendar.FirstWeekday);

	public ICalendarDaySummary GetItem(
		int section,
		int item)
	{
		CalendarMonthSummary month = this[section];
		return item >= 0 && item < month.Items.Count
			? month.Items[item]
			: throw new ArgumentOutOfRangeException(nameof(item));
	}

	
	public IEnumerator<CalendarMonthSummary> GetEnumerator()
	{
		for (int index = 0; index < Count; index++)
			yield return this[index];
	}

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();


	YearMonth MonthAt(
		int index)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(index);
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

		int year = Math.DivRem(index, 12, out int month);
		return new(year + 1, month + 1);
	}
}
