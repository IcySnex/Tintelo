namespace Tintelo.iOS.Models.Journal;

public sealed record Entry(
	Guid Id,
	DateOnly Date,
	Mood Mood,
	string? Note,
	IReadOnlyList<Activity> Activities,
	DateTime CreatedUtc,
	DateTime UpdatedUtc);
