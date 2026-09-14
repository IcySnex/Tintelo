using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Models.About;

namespace Tintelo.iOS.ViewModels.About;

public partial class AboutDependenciesViewModel(
	INavigator navigator)
{
	public Dependency[] Dependencies { get; } =
	[
		new("SkeleKit", "IcySnex", "0.1.15", "https://icysnex.github.io/SkeleKit/"),
		new("CommunityToolkit.Mvvm", "Microsoft & .NET Foundation", "8.4.2", "https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/"),
		new("Microsoft.Data.Sqlite", "Microsoft", "10.0.12", "https://learn.microsoft.com/dotnet/standard/data/sqlite"),
		new("Aigamo.ResXGenerator", "ycanardeau", "4.4.0", "https://github.com/ycanardeau/ResXGenerator")
	];

	
	[RelayCommand(AllowConcurrentExecutions = true)]
	Task OpenDependencyUrlAsync(
		Dependency dependency) =>
		navigator.OpenUrlAsync(dependency.Url);
}