using CommunityToolkit.Mvvm.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsViewModel : ObservableObject
{
	public IReadOnlyList<SettingsSection> Sections { get; } =
	[
		new([
			new("Hello World"),
			new("heaaa")
		]),
		
		new([
			new("Hello World"),
			new("heaaa")
		])
	];
}