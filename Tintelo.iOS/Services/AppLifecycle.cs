using SkeleKit;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.Services;

internal sealed class AppLifecycle(
	AppConfig config) : IApplicationLifecycle
{
	public Task StartAsync()
	{
		config.Theme.Apply();

		return Task.CompletedTask;
	}
}
