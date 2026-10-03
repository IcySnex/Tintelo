using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.Services;

internal sealed class AppLifecycle(
	ILogger<AppLifecycle> logger,
	AppConfig config,
	CurrentDay currentDay) : IApplicationLifecycle
{
	public Task StartAsync()
	{
		logger.LogInformation("App has started.");
		
		config.Theme.Apply();

		return Task.CompletedTask;
	}

	public Task EnterForegroundAsync()
	{
		logger.LogInformation("App has entered foreground.");
		
		currentDay.Refresh();

		return Task.CompletedTask;
	}
}
