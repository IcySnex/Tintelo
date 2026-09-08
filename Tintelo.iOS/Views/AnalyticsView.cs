using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public class AnalyticsView : ContentView<AnalyticsViewModel>
{
	public AnalyticsView(AnalyticsViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Analytics_Title;
		
		ToolbarItems.Add(new()
		{
			Icon = "switch.2",
			Text = Texts.Settings_Title,
			Command = viewModel.OpenSettingsCommand
		});
	}
}
