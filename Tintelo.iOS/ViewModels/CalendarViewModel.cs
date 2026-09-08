using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.ViewModels;

public partial class CalendarViewModel(
	INavigator navigator,
	DatabaseService databaseService,
	BackupService backupService) : ObservableObject
{
	[RelayCommand]
	Task OpenSettingsAsync() =>
		navigator.PresentAsync<SettingsViewModel>(ModalStyle.FormSheet);
	
	
	[RelayCommand]
	async Task ShowDbInfoAsync()
	{
		await databaseService.InitializeAsync();
		
		LibraryMetadata info = await backupService.ReadMetadataAsync(Paths.Database);
		await navigator.AlertAsync(info.LibraryId.ToString(), info.Revision.ToString());
	}
	
}