using SkeleKit;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public class CalendarView : ContentView<CalendarViewModel>
{
	public CalendarView(CalendarViewModel viewModel) : base(viewModel)
	{
		Title = "Calendar";
	}
}