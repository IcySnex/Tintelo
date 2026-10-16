using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SkeleKit;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.ViewModels.Add;

public partial class AddEntryViewModelFactory(
	INavigator navigator,
	CurrentDay currentDay) : ObservableObject
{
	public Task ShowDayAsync(
		DateOnly date)
	{
		AddViewModel vm = SkeleApplication.Current!.Services.GetRequiredService<AddViewModel>();
		vm.Date = date;

		return navigator.PresentAsync(vm, ModalStyle.Sheet(Detent.Content));
	}


	[RelayCommand]
	Task ShowCurrentDayAsync() =>
		ShowDayAsync(currentDay.Value);
}
