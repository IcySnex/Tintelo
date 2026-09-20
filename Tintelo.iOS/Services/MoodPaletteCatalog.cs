using CommunityToolkit.Mvvm.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Models.Palette;

namespace Tintelo.iOS.Services;

public partial class MoodPaletteCatalog : ObservableObject
{
	static MoodPalette.Swatch Swatch(
		uint lightBackground,
		uint darkBackground,
		uint lightForeground,
		uint darkForeground) =>
		new(Color.Dynamic(
				Color.FromHex(lightBackground),
				Color.FromHex(darkBackground)),
			Color.Dynamic(
				Color.FromHex(lightForeground),
				Color.FromHex(darkForeground)));
	
	
	public IReadOnlyDictionary<MoodPaletteId, MoodPalette> Palettes { get; } = new Dictionary<MoodPaletteId, MoodPalette>
	{
		[MoodPaletteId.Default] = new(
			ExtremelyGood: Swatch(0x456D24, 0x3F621F, 0xF3F9E6, 0xF3F9E6),
			VeryGood: Swatch(0xA4BA65, 0xA4BA65, 0x243009, 0x243009),
			Good: Swatch(0xDDE5C2, 0xDDE5C2, 0x3A4A14, 0x3A4A14),
			Neutral: Swatch(0xD4D4D4, 0xD4D4D4, 0x404040, 0x404040),
			Bad: Swatch(0xFBDADD, 0xFBDADD, 0x6E2417, 0x6E2417),
			VeryBad: Swatch(0xF19178, 0xF19178, 0x6E2417, 0x6E2417),
			ExtremelyBad: Swatch(0xA92729, 0xBD3234, 0xFFFFFF, 0xFFFFFF)),
		
		[MoodPaletteId.Pixy] = new(
			ExtremelyGood: Swatch(0x047857, 0x00694B, 0xECFDF5, 0xECFDF5),
			VeryGood: Swatch(0x35D399, 0x35D399, 0x053D2E, 0x053D2E),
			Good: Swatch(0xA7F3D0, 0xA7F3D0, 0x065F46, 0x065F46),
			Neutral: Swatch(0xD4D4D4, 0xE5E5E5, 0x404040, 0x404040),
			Bad: Swatch(0xFFEDD5, 0xFFEDD5, 0x7C2D12, 0x7C2D12),
			VeryBad: Swatch(0xFDBA74, 0xFDBA74, 0x7C2D12, 0x7C2D12),
			ExtremelyBad: Swatch(0xD63C3C, 0xBD2C2C, 0xFFFFFF, 0xFFFFFF))
	};


	[ObservableProperty]
	public partial MoodPalette Current { get; set; } = null!;
}
