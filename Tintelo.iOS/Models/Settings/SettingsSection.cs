using SkeleKit;

namespace Tintelo.iOS.Models.Settings;

public sealed record SettingsSection(
	string Title,
	IReadOnlyList<SettingsEntry> Items) : ISection<SettingsEntry>;