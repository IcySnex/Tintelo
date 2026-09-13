using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsViewModel : ObservableObject
{
	readonly INavigator navigator;


	public SettingsViewModel(
		AppConfig config,
		INavigator navigator)
	{
		this.navigator = navigator;

		Sections =
		[
			new("Theme",
			[
				new SettingsPickerEntry(
					"Appearance",
					"moon",
					["System", "Light", "Dark"],
					BindingFactory.Bind(config, config => config.Theme)
						.Path(theme => theme.Appearance)
						.TwoWay((theme, index) => theme.Appearance = index)),
				
				new SettingsDisplayEntry("Accent Color", "paintbrush", "Default")
			]),
		];
	}


	public IReadOnlyList<SettingsSection> Sections { get; }


	[RelayCommand]
	static async Task ShowAboutAsync()
	{
		await Task.Delay(2000);
		Console.WriteLine("hi");
	}

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
