using SkeleKit;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public class SettingsMoodPaletteContainerCell : ItemView<SettingsMoodPaletteViewModel.MoodPaletteContainer>
{
	public SettingsMoodPaletteContainerCell()
	{
		Background = Colors.SecondaryGroupedBackground;
		
		Content = new Label
		{
			Margin = new(16, 0),
			
			Text = Bind(item => item.Id)
				.ConvertTo(value => value.ToString()),
		};
	}
}