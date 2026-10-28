using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class ActivitySectionHeaderCell : ItemView<ActivitySection>
{
	public ActivitySectionHeaderCell()
	{
		HighlightBackground = null;

		Content = new Label
		{
			Margin = new(16, 12, 16, 8),

			Text = Bind(section => section.IsArchive)
				.ConvertTo(isArchive => isArchive
					? Texts.Settings_Entries_Activities_Archived
					: string.Empty),
			TextColor = Colors.SecondaryLabel,
			FontWeight = FontWeight.Semibold,

			MaxLines = 1
		};
	}
}
