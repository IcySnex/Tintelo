using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.ViewModels.About;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsViewModel(
	AppConfig config,
	INavigator navigator) : ObservableObject
{
	public IReadOnlyList<SettingsSection> Sections { get; } =
	[
		new(Texts.Settings_Theme,
		[
			SettingsPickerEntry.Enum(
				Texts.Settings_Theme_Appearance,
				"moon",
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.Appearance)
					.TwoWay((theme, value) => theme.Appearance = value),
				value => value switch
				{
					Appearance.System => Texts.Settings_Theme_Appearance_System,
					Appearance.Light => Texts.Settings_Theme_Appearance_Light,
					Appearance.Dark => Texts.Settings_Theme_Appearance_Dark,
					_ => throw new ArgumentOutOfRangeException(nameof(value))
				}),
				
			SettingsPickerEntry.Enum(
				Texts.Settings_Theme_Accent,
				"paintbrush",
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.Accent)
					.TwoWay((theme, value) => theme.Accent = value),
				value => value switch
				{
					Accent.Default => Texts.Settings_Theme_Accent_Default,
					Accent.Red => Texts.Settings_Theme_Accent_Red,
					Accent.Orange => Texts.Settings_Theme_Accent_Orange,
					Accent.Yellow => Texts.Settings_Theme_Accent_Yellow,
					Accent.Green =>  Texts.Settings_Theme_Accent_Green,
					Accent.Blue => Texts.Settings_Theme_Accent_Blue,
					Accent.Purple => Texts.Settings_Theme_Accent_Purple,
					Accent.Pink => Texts.Settings_Theme_Accent_Pink,
					Accent.Gray => Texts.Settings_Theme_Accent_Gray,
					_ => throw new ArgumentOutOfRangeException(nameof(value))
				},
				true),
	
			..OperatingSystem.IsIOSVersionAtLeast(27) 
				? [new SettingsToggleEntry(
					Texts.Settings_Theme_SoftScrollEdge,
					"water.waves.and.arrow.trianglehead.down",
					BindingFactory.Bind(config, config => config.Theme)
						.Path(theme => theme.SoftScrollEdge)
						.TwoWay((theme, value) => theme.SoftScrollEdge = value))] 
				: Array.Empty<SettingsEntry>()
		]),
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
