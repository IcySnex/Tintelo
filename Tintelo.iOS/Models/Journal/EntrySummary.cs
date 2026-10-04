namespace Tintelo.iOS.Models.Journal;

public sealed record EntrySummary(
	DateOnly Date,
	Mood Mood,
	bool HasNote);
