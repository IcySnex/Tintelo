using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.ViewModels.Calendar;
using Tintelo.iOS.Views.Calendar.Cells;

namespace Tintelo.iOS.Views.Calendar;

[Page]
public class CalendarView : ContentView<CalendarViewModel>
{
	public CalendarView(CalendarViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Calendar_Title;
		Background = Colors.Background;
		
		NavigationAccessory = new WeekdaysHeader();

		ToolbarItems.Add(new()
		{
			Icon = "switch.2",
			Text = Texts.Settings_Title,
			Command = viewModel.OpenSettingsCommand
		});
		
		Content = new CollectionView<ICalendarDaySummary, CalendarMonthSummary>
		{
			Padding = new(10, 0, 10, 0),
			
			Layout = CollectionLayout.Grid(
				columns: 7,
				spacing: 6,
				itemAspectRatio: 1),
			ShowsSeparators = false,
			RetainsSelection = false,
			
			SectionHeaderTemplate = static () => new CalendarMonthHeaderCell(),
			ItemTemplate = static () => new CalendarDayCell(),
			
			GroupedItemsSource = viewModel.Months,
			ItemCommand = viewModel.OpenDetailsCommand
		};
	}
}
