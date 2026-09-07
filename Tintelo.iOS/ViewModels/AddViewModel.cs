using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;

namespace Tintelo.iOS.ViewModels;

public partial class AddViewModel(
	INavigator navigator) : ObservableObject
{
	[RelayCommand]
	Task ShowAsync() =>
		navigator.PresentAsync<AddViewModel>(ModalStyle.FormSheet);
}
