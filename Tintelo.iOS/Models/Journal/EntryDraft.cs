namespace Tintelo.iOS.Models.Journal;

public sealed record EntryDraft(
	Guid? Id,
	DateOnly Date,
	Mood Mood,
	string? Note,
	IReadOnlyList<Guid> CategoryIds);
