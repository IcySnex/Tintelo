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
		Title = Texts.Calendar;
		Background = Colors.Background;
		
		NavigationAccessory = new WeekdaysHeader(viewModel.Calendar);

		ToolbarItems.Add(new()
		{
			Icon = "switch.2",
			Text = Texts.Settings,
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
			RetainsHighlight = false,
			
			SectionHeaderTemplate = static () => new CalendarMonthHeaderCell(),
			ItemTemplateSelector = new ItemTemplateSelector<ICalendarDaySummary>()
				.Add(static () => new EmptyCalendarDayCell())
				.Add(static () => new CalendarDayCell()),
			
			GroupedItemsSource = Bind(viewModel => viewModel.Months),
			ItemCommand = viewModel.OpenDetailsCommand
		};
	}
}
