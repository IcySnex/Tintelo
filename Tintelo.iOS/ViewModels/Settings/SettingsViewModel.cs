using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Settings;

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
					.TwoWay((theme, appearance) => theme.Appearance = appearance),
				appearance => appearance switch
				{
					Appearance.System => Texts.Settings_Theme_Appearance_System,
					Appearance.Light => Texts.Settings_Theme_Appearance_Light,
					Appearance.Dark => Texts.Settings_Theme_Appearance_Dark,
					_ => throw new ArgumentOutOfRangeException(nameof(appearance))
				}),
				
			SettingsPickerEntry.Enum(
				Texts.Settings_Theme_Accent,
				"paintbrush",
				BindingFactory.Bind(config, config => config.Theme)
					.Path(theme => theme.Accent)
					.TwoWay((theme, accent) => theme.Accent = accent),
				accent => accent switch
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
					_ => throw new ArgumentOutOfRangeException(nameof(accent))
				},
				true)
		]),
	];


	[RelayCommand]
	Task ShowAboutAsync() =>
		navigator.PushAsync<About.AboutViewModel>();

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
