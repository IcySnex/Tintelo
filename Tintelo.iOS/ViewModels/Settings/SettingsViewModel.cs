using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.About;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsViewModel(
	AppConfig config,
	INavigator navigator) : ObservableObject
{
	static SettingsEntry[] OnIOSVersionAtLeast(
		int major,
		SettingsEntry entry) =>
		OperatingSystem.IsIOSVersionAtLeast(major)
			? [entry]
			: Array.Empty<SettingsEntry>();

	
	public IReadOnlyList<SettingsSection> Sections { get; } =
	[
		new(Texts.Settings_Theme,
		[
			new SettingsNavigationEntry(
				Texts.Settings_Theme_MoodPalette,
				"swatchpalette",
				typeof(SettingsMoodPaletteViewModel),
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.MoodPalette)
					.ConvertTo(SettingsDisplays.MoodPalettes.GetTitle)),
			
			new SettingsPickerEntry(
				Texts.Settings_Theme_Appearance,
				"moon",
				SettingsDisplays.Appearances.Titles,
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.Appearance)
					.ConvertTo(SettingsDisplays.Appearances.GetTitle)
					.ConvertFrom(SettingsDisplays.Appearances.GetValue)
					.TwoWay((theme, value) => theme.Appearance = value),
				Colors.SecondaryLabel),
			
			new SettingsPickerEntry(
				Texts.Settings_Theme_Accent,
				"paintbrush",
				SettingsDisplays.Accents.Titles,
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.Accent)
					.ConvertTo(SettingsDisplays.Accents.GetTitle)
					.ConvertFrom(SettingsDisplays.Accents.GetValue)
					.TwoWay((theme, value) => theme.Accent = value)),
	
			..OnIOSVersionAtLeast(27, new SettingsToggleEntry(
				Texts.Settings_Theme_SoftScrollEdge,
				"water.waves.and.arrow.trianglehead.down",
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.SoftScrollEdge)
					.TwoWay((theme, value) => theme.SoftScrollEdge = value)))
		])
	];


	[RelayCommand]
	Task ShowAboutAsync() =>
		navigator.PushAsync<AboutViewModel>();

	[RelayCommand(AllowConcurrentExecutions = true)]
	async Task ActivateAsync(
		SettingsEntry entry)
	{
		switch (entry)
		{
			case SettingsActionEntry action when action.Command.CanExecute(null):
				await action.Command.ExecuteAsync(null);
				break;

			case SettingsNavigationEntry navigation:
				await navigator.PushAsync(navigation.ViewModelType);
				break;
		}
	}
}
