using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Logging;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels;
using Tintelo.iOS.ViewModels.Settings;
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
		services.AddSingleton<SimpleStorage>();
		services.AddSingleton<DatabaseService>();
		services.AddSingleton<BackupService>();
		
		services.AddSingleton<AppConfig>();

		services.AddSingleton<SettingsViewModel>();
		services.AddSingleton<CalendarViewModel>();
		services.AddSingleton<AnalyticsViewModel>();
		services.AddSingleton<AddViewModel>();
	})
	.Tabs(configure => configure
		.LargeTitles()
		.Tab<CalendarView>(Texts.Calendar_Title, ImageSource.Symbol($"{DateTime.Now.Day}.calendar"))
		.Tab<AnalyticsView>(Texts.Analytics_Title, ImageSource.Symbol("chart.bar.xaxis"))
		.Bubble<AddViewModel>(Texts.AddEntry_Title, ImageSource.Symbol("plus"), vm => vm.ShowCommand))
	.UseTint(Color.Dynamic(
		Color.FromHex(0x658631),
		Color.FromHex(0xb9cc7a)))
	.Build()
	.Run(args);
