using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.Services;

internal sealed class AppLifecycle(
	ILogger<AppLifecycle> logger,
	AppConfig config,
	CurrentDay currentDay,
	CalendarProvider calendar) : IApplicationLifecycle
{
	public async Task StartAsync()
	{
		logger.LogInformation("App has started.");

		config.Theme.Apply();

		await calendar.LoadAsync();
	}

	public Task EnterForegroundAsync()
	{
		logger.LogInformation("App has entered foreground.");
		
		currentDay.Refresh();

		return Task.CompletedTask;
	}
}
