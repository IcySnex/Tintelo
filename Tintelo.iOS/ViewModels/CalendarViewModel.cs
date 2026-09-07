using CommunityToolkit.Mvvm.ComponentModel;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.ViewModels;

public class CalendarViewModel(
	AppConfig appConfig) : ObservableObject
{
	public AppConfig AppConfig => appConfig;
}