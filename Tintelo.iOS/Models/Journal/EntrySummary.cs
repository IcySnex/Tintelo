namespace Tintelo.iOS.Models.Journal;

public readonly record struct EntrySummary(
	DateOnly Date,
	Mood Mood,
	bool HasNote);
