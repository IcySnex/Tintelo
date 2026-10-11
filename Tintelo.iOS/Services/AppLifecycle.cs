using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.ViewModels.Calendar;

namespace Tintelo.iOS.Services;

internal sealed class AppLifecycle(
	ILogger<AppLifecycle> logger,
	INavigator navigator,
	ISplitView split,
	AppConfig config,
	CalendarDetailsViewModel details,
	CurrentDay currentDay,
	CalendarProvider calendar) : IApplicationLifecycle
{
	public async Task StartAsync()
	{
		logger.LogInformation("App has started.");

		config.Theme.Apply();

		split.CollapsedChanged += collapsed =>
		{
			async Task DockDetailsAsync()
			{
				if (await navigator.TryDismissAsync(details))
					split.Show(SplitViewColumn.Primary);
			}

			if (!collapsed)
				_ = DockDetailsAsync();
		};

		await calendar.LoadAsync();
	}

	public Task EnterForegroundAsync()
	{
		logger.LogInformation("App has entered foreground.");
		
		currentDay.Refresh();

		return Task.CompletedTask;
	}
}
