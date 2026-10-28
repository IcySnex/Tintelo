using SkeleKit;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.Models.Settings;

public class ActivitySection(
	IEnumerable<Activity> items) : ISection<Activity>
{
	public virtual bool IsArchive => false;
	
	public ObservableRangeCollection<Activity> Items { get; } = new(items);
	
	IReadOnlyList<Activity> ISection<Activity>.Items => Items;
}

public sealed class ActivityArchiveSection(
	IEnumerable<Activity> items,
	bool isExpanded = false) : ActivitySection(items), IExpandableSection<Activity>
{
	public override bool IsArchive => true;

	public bool IsExpanded { get; set; } = isExpanded;
}
