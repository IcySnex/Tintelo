using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.About;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.About;

namespace Tintelo.iOS.Views.About;

[Page]
public class AboutDependenciesView : ContentView<AboutDependenciesViewModel>
{
	public AboutDependenciesView(
		AboutDependenciesViewModel viewModel) : base(viewModel)
	{
		StackPanel stack;
		
		
		Title = Texts.About_Dependencies;
		Background = LinearGradient.Vertical(Colors.Gray5, Colors.Gray6);

		Content = new ScrollView
		{
			SystemInsetEdges = LayoutEdges.Horizontal | LayoutEdges.Top,
			
			Content = stack = new StackPanel
			{
				Spacing = 8,

				Children =
				{
					new Label
					{
						Text = Texts.About_Dependencies_Thanks,
						MaxLines = 10,
						Truncation = Truncation.Tail
					}
				}
			}
		};
		
		foreach (Dependency dependency in viewModel.Dependencies)
			stack.Children.Add(new Button
			{
				Padding = new(0, 12),
				
				Text = dependency.Title,
				Subtitle = Texts.About_Dependencies_Dependency_Description.Format(dependency.Version, dependency.Author),
				
				Kind = ButtonStyle.Tinted,
				
				Command = viewModel.OpenDependencyUrlCommand,
				CommandParameter = dependency
			});
	}
}