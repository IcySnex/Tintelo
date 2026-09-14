using System.Reflection;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;

namespace Tintelo.iOS.ViewModels.About;

public partial class AboutViewModel(
	INavigator navigator)
{
	public const string ContactEmail = "lao43919@gmail.com";
	
	
	public string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.0.0";


	[RelayCommand]
	Task ShowTermsAsync() =>
		navigator.PushAsync<AboutTermsViewModel>();

	[RelayCommand]
	Task ShowDependenciesAsync() =>
		navigator.PushAsync<AboutDependenciesViewModel>();


	[RelayCommand]
	async Task ContactAsync()
	{
		const string body = """
		                     hallo world
		                     ha
		                     """;

		await navigator.AlertAsync("Contact", body); // show email composer
	}
}