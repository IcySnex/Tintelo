using Tintelo.iOS.Models.Journal;

namespace Tintelo.iOS.Models.AddEntry;

public class EntryDraft(
	DateOnly date)
{
	public DateOnly Date { get; } = date;

	public Mood? Mood { get; set; }
}