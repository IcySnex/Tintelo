using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Palette;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsMoodPaletteViewModel : ObservableObject
{
	public record MoodPaletteContainer(
		MoodPaletteId Id,
		MoodPalette Palette);


	readonly AppConfig config;
	readonly INavigator navigator;

	public SettingsMoodPaletteViewModel(
		AppConfig config,
		INavigator navigator,
		MoodPaletteCatalog moodPaletteCatalog)
	{
		this.config = config;
		this.navigator = navigator;
		
		Palettes = moodPaletteCatalog.Palettes
			.Select(pair => new MoodPaletteContainer(pair.Key, pair.Value))
			.ToArray();
		SelectedPalette = new(config.Theme.MoodPalette, moodPaletteCatalog.Current);
	}
	
	
	public MoodPaletteContainer[] Palettes { get; }
	
    [ObservableProperty]
    public partial MoodPaletteContainer SelectedPalette { get; set; }


    partial void OnSelectedPaletteChanged(MoodPaletteContainer value)
    {
	    config.Theme.MoodPalette = value.Id;
	    _ = navigator.PopAsync();
    }
}