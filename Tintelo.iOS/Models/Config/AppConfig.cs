using Microsoft.Extensions.Logging;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public sealed class AppConfig(
	ILogger<AppConfig> logger,
	SimpleStorage storage,
	MoodPaletteCatalog moodPaletteCatalog)
{
	public AppThemeConfig Theme { get; } = new(logger, storage, moodPaletteCatalog);
}
