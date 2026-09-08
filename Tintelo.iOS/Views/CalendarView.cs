using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public class CalendarView : ContentView<CalendarViewModel>
{
	public CalendarView(CalendarViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Calendar_Title;

		Content = new StackPanel()
		{
			VerticalAlignment = VerticalAlignment.Center,

			Children =
			{
				new Button
				{
					Text = "Show DB info",
					Command = viewModel.ShowDbInfoCommand
				}
			}
		};
	}
}
