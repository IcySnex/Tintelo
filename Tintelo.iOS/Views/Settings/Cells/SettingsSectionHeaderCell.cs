using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsSectionHeaderCell : ItemView<SettingsSection>
{
	public SettingsSectionHeaderCell()
	{
		HighlightBackground = null;

		Content = new Label
		{
			Margin = new(16, 12, 16, 8),

			Text = Bind(section => section.Title),
			TextColor = Colors.SecondaryLabel,
			FontWeight = FontWeight.Semibold,
			
			MaxLines = 1
		};
	}
}
