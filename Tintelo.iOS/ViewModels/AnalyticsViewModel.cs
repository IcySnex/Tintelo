using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels;

public partial class AnalyticsViewModel(
	INavigator navigator) : ObservableObject
{
	[RelayCommand]
	Task OpenSettingsAsync() =>
		navigator.PresentAsync<SettingsViewModel>(ModalStyle.FormSheet);
}