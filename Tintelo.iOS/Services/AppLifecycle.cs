using SkeleKit;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.Services;

internal sealed class AppLifecycle(
	AppConfig config) : IApplicationLifecycle
{
	public Task StartAsync()
	{
		config.Theme.ApplyAppearance();
		config.Theme.ApplyAccent();

		return Task.CompletedTask;
	}
}
