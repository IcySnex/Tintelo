using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public sealed class AppConfig(
	SimpleStorage storage,
	MoodPaletteCatalog moodPaletteCatalog)
{
	public AppThemeConfig Theme { get; } = new(storage, moodPaletteCatalog);
}
