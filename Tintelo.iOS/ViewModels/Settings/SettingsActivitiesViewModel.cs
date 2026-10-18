using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsActivitiesViewModel(
	INavigator navigator) : ObservableObject
{
    [ObservableProperty]
    public partial bool IsEditing { get; set; }

    
    [RelayCommand]
	Task ShowArchiveAsync() =>
		Task.CompletedTask;
	
	[RelayCommand]
	Task AddAsync() =>
		Task.CompletedTask;
}