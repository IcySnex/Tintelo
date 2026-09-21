using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Models.Palette;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public partial class AppThemeConfig : ObservableObject
{
	readonly ILogger<AppConfig> logger;
	readonly MoodPaletteCatalog moodPaletteCatalog;
	
	public AppThemeConfig(
		ILogger<AppConfig> logger,
		SimpleStorage storage,
		MoodPaletteCatalog moodPaletteCatalog)
	{
		this.logger = logger;
		this.moodPaletteCatalog = moodPaletteCatalog;
		
		InitializeStoredProperties(storage);
	}
	
	
	[ObservableProperty]
	[StoreAs("configuration.theme.moodpalette", MoodPaletteId.Default)]
	public partial MoodPaletteId MoodPalette { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.theme.appearance", Appearance.System)]
	public partial Appearance Appearance { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.theme.accent", Accent.Default)]
	public partial Accent Accent { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.theme.softscrolledge", false)]
	public partial bool SoftScrollEdge { get; set; }


	partial void OnMoodPaletteChanged(MoodPaletteId value) =>
		ApplyMoodPalette();

	partial void OnAppearanceChanged(Appearance value) =>
		ApplyAppearance();

	partial void OnAccentChanged(Accent value) =>
		ApplyAccent();

	partial void OnSoftScrollEdgeChanged(bool value) =>
		ApplySoftScrollEdge();


	void ApplyMoodPalette()
	{
		logger.LogInformation("Applying mood palette...");
		moodPaletteCatalog.Current = moodPaletteCatalog.Palettes[MoodPalette];
	}

	void ApplyAppearance()
	{
		logger.LogInformation("Applying appearance...");
		SkeleApplication.Current?.Theme.Appearance = Appearance;
	}

	void ApplyAccent()
	{
		logger.LogInformation("Applying accent...");
		SkeleApplication.Current?.Theme.Tint = Accent switch
		{
			Accent.Default => Color.Dynamic(Color.FromHex(0x658631), Color.FromHex(0xb9cc7a)),
			Accent.Red => Color.Dynamic(Color.FromHex(0xe15b5b), Color.FromHex(0xff7b7b)),
			Accent.Orange => Color.Dynamic(Color.FromHex(0xf9833f), Color.FromHex(0xff8c2a)),
			Accent.Yellow => Color.Dynamic(Color.FromHex(0xe59b24), Color.FromHex(0xfde047)),
			Accent.Green => Color.Dynamic(Color.FromHex(0x48a968), Color.FromHex(0x6ee7b7)),
			Accent.Blue => Color.Dynamic(Color.FromHex(0x007ff5), Color.FromHex(0x1daeff)),
			Accent.Purple  => Color.Dynamic(Color.FromHex(0x9061f9), Color.FromHex(0xc084fc)),
			Accent.Pink => Color.Dynamic(Color.FromHex(0xed5e93), Color.FromHex(0xf472b6)),
			Accent.Gray => Color.Dynamic(Color.FromHex(0x64748b), Color.FromHex(0x94a3b8)),
			_ => throw new ArgumentOutOfRangeException(nameof(Accent))
		};
	}

	void ApplySoftScrollEdge()
	{
		logger.LogInformation("Applying soft scroll edge...");
		SkeleApplication.Current?.Theme.TopScrollEdgeStyle = SoftScrollEdge
			? ScrollEdgeStyle.Soft
			: ScrollEdgeStyle.Automatic;
	}


	public void Apply()
	{
		ApplyMoodPalette();
		ApplyAppearance();
		ApplyAccent();
		ApplySoftScrollEdge();
	}
}