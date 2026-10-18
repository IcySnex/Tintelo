using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page(Singleton = true)]
public class AnalyticsView : ContentView<AnalyticsViewModel>
{
	public AnalyticsView(AnalyticsViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Analytics;
		
		ToolbarItems.Add(new ToolbarButton
		{
			Command = viewModel.OpenSettingsCommand,
			
			Icon = ImageSource.Symbol("switch.2"),
			Text = Texts.Settings,
			Tint = BindingFactory.Bind(SkeleApplication.Current!.Theme, theme => theme.Tint)
		});
	}
}
