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
				new TextField()
				{
					Text = Bind(vm => vm.AppConfig)
						.Path(appConfig => appConfig.StringValue)
						.TwoWay((appConfig, val) => appConfig.StringValue = val)
				},
				new Switch()
				{
					IsOn = Bind(vm => vm.AppConfig)
						.Path(appConfig => appConfig.BoolValue)
						.TwoWay((appConfig, val) => appConfig.BoolValue = val)
				},
				new Stepper()
				{
					Value = Bind(vm => vm.AppConfig)
						.Path(appConfig => appConfig.IntValue)
						.ConvertTo(val => (double)val)
						.ConvertFrom(val => (int)val)
						.TwoWay((appConfig, val) => appConfig.IntValue = val)
				}
			}
		};
	}
}
