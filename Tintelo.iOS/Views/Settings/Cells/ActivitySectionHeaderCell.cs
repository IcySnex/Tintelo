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
			IsVisible = Bind(section => section.IsArchive),
			
			Margin = new(16, 12, 16, 8),

			Text = Texts.Settings_Entries_Activities_Archived,
			TextColor = Colors.SecondaryLabel,
			FontWeight = FontWeight.Semibold,

			MaxLines = 1
		};
	}
}
