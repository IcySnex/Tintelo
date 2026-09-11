using SkeleKit;

namespace Tintelo.iOS.Models.Settings;

public sealed record SettingsSection(
	IReadOnlyList<SettingsEntry> Items) : ISection<SettingsEntry>;