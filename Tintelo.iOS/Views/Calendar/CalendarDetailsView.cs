using SkeleKit;
using Tintelo.iOS.ViewModels.Calendar;

namespace Tintelo.iOS.Views.Calendar;

[Page]
public class CalendarDetailsView : ContentView<CalendarDetailsViewModel>
{
	public CalendarDetailsView(CalendarDetailsViewModel viewModel) : base(viewModel)
	{
		Title = "Details";
	}
}