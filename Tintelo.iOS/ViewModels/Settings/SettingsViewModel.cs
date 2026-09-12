using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsViewModel(
	INavigator navigator) : ObservableObject
{
	public IReadOnlyList<SettingsEntry> Items { get; } =
	[
		new("Email", "envelope"),
		new("Hello World", "square.and.arrow.down"),
		new("dihbidus bisud bdsui bidus bdi subsd uibdisu b", "dot.square"),
	];


	[RelayCommand]
	Task ShowAboutAsync() =>
		navigator.AlertAsync("hii", ":3");
}
