using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Logging;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels;
using Tintelo.iOS.Views;

SkeleApplication.CreateBuilder()
	.ConfigureLogging(logging =>
	{
		logging.SetMinimumLevel(LogLevel.Information);

		logging.AddConsole();
		logging.AddFile(Paths.Logs);
	})
	.UseServices(services =>
	{
		services.AddSingleton<CalendarViewModel>();
		services.AddSingleton<AnalyticsViewModel>();
		
		services.AddSingleton<AddViewModel>();
	})
	.Tabs(configure => configure
		.Tab<CalendarView>("Calendar", ImageSource.Symbol($"{DateTime.Now.Day}.calendar"))
		.Tab<AnalyticsView>("Analytics", ImageSource.Symbol("chart.bar.xaxis"))
		.Bubble<AddViewModel>("Add Entry", ImageSource.Symbol("plus"), vm => vm.ShowCommand)
		.LargeTitles())
	.UseTint(Color.Dynamic(
		Color.FromHex(0x658631),
		Color.FromHex(0xb9cc7a)))
	.Build()
	.Run(args);
