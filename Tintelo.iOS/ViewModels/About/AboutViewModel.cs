using System.Reflection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Logging;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.ViewModels.About;

public partial class AboutViewModel(
	ILogger<AboutViewModel> logger,
	INavigator navigator,
	IMailer mailer,
	SystemInfo systemInfo)
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
		try
		{
			logger.LogInformation("Showing mail composer...");

			MailContent content = new()
			{
				To = { ContactEmail },
				Subject = Texts.About_Contact_Mail_Subject,
				Body = Texts.About_Contact_Mail_Body.Format(
					Version,
					systemInfo.GetDeviceModel(),
					systemInfo.GetOperatingSystem(),
					systemInfo.GetBatteryLevel())
			};

			if (LogFiles.GetLatestPath() is string logPath)
				content.Attachments.Add(new()
				{
					Data = await File.ReadAllBytesAsync(logPath),
					FileName = Path.GetFileName(logPath),
					MimeType = "text/plain"
				});

			await mailer.ComposeAsync(content);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to compose mail.");
			await navigator.AlertAsync(Texts.Error, Texts.Error_Description_Mail);
		}
	}
}