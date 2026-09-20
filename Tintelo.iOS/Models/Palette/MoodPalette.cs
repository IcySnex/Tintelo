using SkeleKit;

namespace Tintelo.iOS.Models.Palette;

public record MoodPalette(
	MoodPalette.Swatch ExtremelyGood,
	MoodPalette.Swatch VeryGood,
	MoodPalette.Swatch Good,
	MoodPalette.Swatch Neutral,
	MoodPalette.Swatch Bad,
	MoodPalette.Swatch VeryBad,
	MoodPalette.Swatch ExtremelyBad)
{
	public record Swatch(
		Color Background,
		Color Foreground);
	

	public static Swatch Get(
		MoodPalette palette,
		Mood mood) =>
		mood switch
		{
			Mood.ExtremelyGood => palette.ExtremelyGood,
			Mood.VeryGood => palette.VeryGood,
			Mood.Good => palette.Good,
			Mood.Neutral => palette.Neutral,
			Mood.Bad => palette.Bad,
			Mood.VeryBad => palette.VeryBad,
			Mood.ExtremelyBad => palette.ExtremelyBad,
			_ => throw new ArgumentOutOfRangeException(nameof(mood))
		};
}