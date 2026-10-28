namespace Tintelo.iOS.Models.Journal;

public sealed record Activity(
	Guid Id,
	string Name,
	ActivityIcon? Icon,
	string Color,
	bool IsArchived,
	string SortOrder);
