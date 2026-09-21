using System.Runtime.CompilerServices;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Palette;
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
		HighlightBackground = Colors.TertiaryBackground;

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
						Circle(item => item.Palette.ExtremelyGood.Background),
					}
				},
				
				new Label
				{
					Text = Bind(item => item.Id)
						.Once()
						.ConvertTo(value => value switch
						{
							MoodPaletteId.Default => Texts.Settings_Theme_MoodPalette_Default,
							MoodPaletteId.Pixy => Texts.Settings_Theme_MoodPalette_Pixy,
							MoodPaletteId.RedGreenCvd => Texts.Settings_Theme_MoodPalette_RedGreenCvd,
							MoodPaletteId.BlueYellowCvd => Texts.Settings_Theme_MoodPalette_BlueYellowCvd,
							MoodPaletteId.Monochrome => Texts.Settings_Theme_MoodPalette_Monochrome,
							_ => throw new ArgumentOutOfRangeException(nameof(value))
						}),
					TextStyle = TextStyle.Callout,
					FontWeight = FontWeight.Medium,
					
				},
				new Label
				{
					Text = Bind(item => item.Id)
						.Once()
						.ConvertTo(value => value switch
						{
							MoodPaletteId.Default => Texts.Settings_Theme_MoodPalette_Default_Description,
							MoodPaletteId.Pixy => Texts.Settings_Theme_MoodPalette_Pixy_Description,
							MoodPaletteId.RedGreenCvd => Texts.Settings_Theme_MoodPalette_RedGreenCvd_Description,
							MoodPaletteId.BlueYellowCvd => Texts.Settings_Theme_MoodPalette_BlueYellowCvd_Description,
							MoodPaletteId.Monochrome => Texts.Settings_Theme_MoodPalette_Monochrome_Description,
							_ => throw new ArgumentOutOfRangeException(nameof(value))
						}),
					TextStyle = TextStyle.Footnote,
					TextColor = Colors.SecondaryLabel
				},

			}
		};
	}

}