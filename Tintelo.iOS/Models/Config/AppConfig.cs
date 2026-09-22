using Microsoft.Extensions.Logging;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public sealed class AppConfig(
	ILogger<AppConfig> logger,
	SimpleStorage storage,
	MoodPaletteCatalog moodPaletteCatalog)
{
	public AppCalendarConfig Calendar { get; } = new(storage);
	
	public AppThemeConfig Theme { get; } = new(logger, storage, moodPaletteCatalog);
}
