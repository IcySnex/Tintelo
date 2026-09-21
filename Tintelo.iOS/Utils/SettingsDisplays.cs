using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Palette;

namespace Tintelo.iOS.Utils;

internal static class SettingsDisplays
{
	public static readonly EnumDisplay<Appearance> Appearances = new(
		value => value switch
		{
			Appearance.System => Texts.Settings_Theme_Appearance_System,
			Appearance.Light => Texts.Settings_Theme_Appearance_Light,
			Appearance.Dark => Texts.Settings_Theme_Appearance_Dark,
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		});

	public static readonly EnumDisplay<Accent> Accents = new(
		value => value switch
		{
			Accent.Default => Texts.Settings_Theme_Accent_Default,
			Accent.Red => Texts.Settings_Theme_Accent_Red,
			Accent.Orange => Texts.Settings_Theme_Accent_Orange,
			Accent.Yellow => Texts.Settings_Theme_Accent_Yellow,
			Accent.Green => Texts.Settings_Theme_Accent_Green,
			Accent.Blue => Texts.Settings_Theme_Accent_Blue,
			Accent.Purple => Texts.Settings_Theme_Accent_Purple,
			Accent.Pink => Texts.Settings_Theme_Accent_Pink,
			Accent.Gray => Texts.Settings_Theme_Accent_Gray,
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		});

	public static readonly EnumDisplay<MoodPaletteId> MoodPalettes = new(
		value => value switch
		{
			MoodPaletteId.Default => Texts.Settings_Theme_MoodPalette_Default,
			MoodPaletteId.Pixy => Texts.Settings_Theme_MoodPalette_Pixy,
			MoodPaletteId.RedGreenCvd => Texts.Settings_Theme_MoodPalette_RedGreenCvd,
			MoodPaletteId.BlueYellowCvd => Texts.Settings_Theme_MoodPalette_BlueYellowCvd,
			MoodPaletteId.Monochrome => Texts.Settings_Theme_MoodPalette_Monochrome,
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		},
		value => value switch
		{
			MoodPaletteId.Default => Texts.Settings_Theme_MoodPalette_Default_Description,
			MoodPaletteId.Pixy => Texts.Settings_Theme_MoodPalette_Pixy_Description,
			MoodPaletteId.RedGreenCvd => Texts.Settings_Theme_MoodPalette_RedGreenCvd_Description,
			MoodPaletteId.BlueYellowCvd => Texts.Settings_Theme_MoodPalette_BlueYellowCvd_Description,
			MoodPaletteId.Monochrome => Texts.Settings_Theme_MoodPalette_Monochrome_Description,
			_ => throw new ArgumentOutOfRangeException(nameof(value))
		});
}
