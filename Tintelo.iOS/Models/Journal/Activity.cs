namespace Tintelo.iOS.Models.Journal;

public sealed record Activity(
	Guid Id,
	string Name,
	string? Emoji,
	string Color,
	bool IsArchived);
