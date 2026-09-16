using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Logging;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels;
using Tintelo.iOS.ViewModels.About;
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
		// Config
		services.AddSingleton<AppConfig>();

		// Services
		services.AddSingleton<SimpleStorage>();
		services.AddSingleton<DatabaseService>();
		services.AddSingleton<BackupService>();
		services.AddSingleton<SystemInfo>();
		
		// ViewModels
		services.AddSingleton<CalendarViewModel>();
		services.AddSingleton<AnalyticsViewModel>();
		services.AddSingleton<AddViewModel>();
		
		services.AddSingleton<SettingsViewModel>();
		
		services.AddSingleton<AboutViewModel>();
		services.AddSingleton<AboutTermsViewModel>();
		services.AddSingleton<AboutDependenciesViewModel>();
	})
	.UseLifecycle<AppLifecycle>()
	.Tabs(configure => configure
		.Tab<CalendarView>(Texts.Calendar_Title, ImageSource.Symbol(OperatingSystem.IsIOSVersionAtLeast(26) ? $"{DateTime.Now.Day}.calendar" : "calendar"))
		.Tab<AnalyticsView>(Texts.Analytics_Title, ImageSource.Symbol("chart.bar.xaxis"))
		.Bubble<AddViewModel>(Texts.AddEntry_Title, ImageSource.Symbol("plus"), vm => vm.ShowCommand))
	.Build()
	.Run(args);
