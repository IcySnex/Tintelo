namespace Tintelo.iOS.Models.Journal;

public sealed record Category(
	Guid Id,
	string Name,
	string? Emoji,
	string Color,
	bool IsArchived);
