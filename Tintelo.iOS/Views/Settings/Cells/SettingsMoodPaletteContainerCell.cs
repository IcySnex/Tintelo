using System.Runtime.CompilerServices;
using SkeleKit;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public class SettingsMoodPaletteContainerCell : ItemView<SettingsMoodPaletteViewModel.MoodPaletteContainer>
{
	static Border Circle(
		Func<SettingsMoodPaletteViewModel.MoodPaletteContainer, Color> read,
		[CallerArgumentExpression(nameof(read))] string? path = null) =>
		new()
		{
			CornerRadius = double.PositiveInfinity,

			Background = Bind(read, path).Once()
		};
	
	
	public SettingsMoodPaletteContainerCell()
	{
		Background = Colors.SecondaryGroupedBackground;

		Content = new StackPanel
		{
			Margin = new Thickness(24, 16),
			VerticalAlignment = VerticalAlignment.Center,
			
			Children =
			{
				new SquareRowPanel()
				{
					Margin = new Thickness(0, 0, 0, 12),
					HorizontalAlignment = HorizontalAlignment.Start,
					
					Spacing = 12,
					MaxItemSize = 44,

					Children =
					{
						Circle(item => item.Palette.ExtremelyBad.Background),
						Circle(item => item.Palette.VeryBad.Background),
						Circle(item => item.Palette.Bad.Background),
						Circle(item => item.Palette.Neutral.Background),
						Circle(item => item.Palette.Good.Background),
						Circle(item => item.Palette.VeryGood.Background),
						Circle(item => item.Palette.ExtremelyGood.Background)
					}
				},
				
				new Label
				{
					Text = Bind(item => item.Id)
						.Once()
						.ConvertTo(SettingsDisplays.MoodPalettes.GetTitle),
					TextStyle = TextStyle.Callout,
					FontWeight = FontWeight.Medium
					
				},
				new Label
				{
					Text = Bind(item => item.Id)
						.Once()
						.ConvertTo(SettingsDisplays.MoodPalettes.GetDescription),
					TextStyle = TextStyle.Footnote,
					TextColor = Colors.SecondaryLabel
				}
			}
		};
	}

}