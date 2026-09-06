using SkeleKit;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public class AnalyticsView : ContentView<AnalyticsViewModel>
{
	public AnalyticsView(AnalyticsViewModel viewModel) : base(viewModel)
	{
		Title = "Analytics";
	}
}