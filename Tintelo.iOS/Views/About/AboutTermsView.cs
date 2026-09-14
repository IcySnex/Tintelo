using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels.About;

namespace Tintelo.iOS.Views.About;

[Page]
public class AboutTermsView : ContentView<AboutTermsViewModel>
{
	public AboutTermsView(AboutTermsViewModel viewModel) : base(viewModel)
	{
		Title = Texts.About_Terms;
		TitleStyle = TitleStyle.Inline;

		Background = LinearGradient.Vertical(Colors.Gray5, Colors.Gray6);

		Content = new ScrollView
		{
			SystemInsetEdges = LayoutEdges.Horizontal | LayoutEdges.Top,
			
			Content = new TextView
			{
				IsSelectable = true,
				Spans = viewModel.Text
			}
		};
	}
}