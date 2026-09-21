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
			ExtremelyBad: Swatch(0xD63C3C, 0xBD2C2C, 0xFFFFFF, 0xFFFFFF)),
		
		[MoodPaletteId.RedGreenCvd] = new(
			ExtremelyGood: Swatch(0x542788, 0x542788, 0xFFFFFF, 0xFFFFFF),
			VeryGood: Swatch(0x998EC3, 0x998EC3, 0x261B36, 0x261B36),
			Good: Swatch(0xD8DAEB, 0xD8DAEB, 0x3F1D5C, 0x3F1D5C),
			Neutral: Swatch(0xD4D4D4, 0xD4D4D4, 0x404040, 0x404040),
			Bad: Swatch(0xFEE0B6, 0xFEE0B6, 0x5A2A00, 0x5A2A00),
			VeryBad: Swatch(0xF1A340, 0xF1A340, 0x4A2500, 0x4A2500),
			ExtremelyBad: Swatch(0xB35806, 0xB35806, 0xFFFFFF, 0xFFFFFF)),
		
		[MoodPaletteId.BlueYellowCvd] = new(
			ExtremelyGood: Swatch(0x3B7A16, 0x3B7A16, 0xFFFFFF, 0xFFFFFF),
			VeryGood: Swatch(0xA1D76A, 0xA1D76A, 0x2A3A12, 0x2A3A12),
			Good: Swatch(0xE6F5D0, 0xE6F5D0, 0x23430F, 0x23430F),
			Neutral: Swatch(0xD4D4D4, 0xD4D4D4, 0x404040, 0x404040),
			Bad: Swatch(0xFDE0EF, 0xFDE0EF, 0x5A0C38, 0x5A0C38),
			VeryBad: Swatch(0xE9A3C9, 0xE9A3C9, 0x5A0C38, 0x5A0C38),
			ExtremelyBad: Swatch(0xC51B7D, 0xC51B7D, 0xFFFFFF, 0xFFFFFF)),
		
		[MoodPaletteId.Monochrome] = new(
			ExtremelyGood: Swatch(0xF1F1F1, 0xFAFAFA, 0x262626, 0x262626),
			VeryGood: Swatch(0xE5E5E5, 0xE5E5E5, 0x262626, 0x262626),
			Good: Swatch(0xD4D4D4, 0xD4D4D4, 0x262626, 0x262626),
			Neutral: Swatch(0xA3A3A3, 0xA3A3A3, 0x171717, 0x171717),
			Bad: Swatch(0x737373, 0x737373, 0xFFFFFF, 0xFFFFFF),
			VeryBad: Swatch(0x525252, 0x525252, 0xFFFFFF, 0xFFFFFF),
			ExtremelyBad: Swatch(0x262626, 0x262626, 0xFFFFFF, 0xFFFFFF))
	};


	[ObservableProperty]
	public partial MoodPalette Current { get; set; } = null!;
}
