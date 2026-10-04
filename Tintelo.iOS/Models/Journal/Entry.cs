namespace Tintelo.iOS.Models.Journal;

public sealed record Entry(
	Guid Id,
	DateOnly Date,
	Mood Mood,
	string? Note,
	IReadOnlyList<Category> Categories,
	DateTime CreatedUtc,
	DateTime UpdatedUtc);
