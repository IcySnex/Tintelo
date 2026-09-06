using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SkeleKit;

namespace Tintelo.iOS.ViewModels;

public partial class AddViewModel(
	ILogger<AddViewModel> logger,
	INavigator navigator) : ObservableObject
{
	[RelayCommand]
	Task ShowAsync()
	{
		logger.LogInformation("grrrr");
		return navigator.PresentAsync<AddViewModel>(ModalStyle.FormSheet);
	}
}