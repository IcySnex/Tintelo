using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.About;

namespace Tintelo.iOS.Views.About;

[Page]
public class AboutView : ContentView<AboutViewModel>
{
	public AboutView(AboutViewModel viewModel) : base(viewModel)
	{
		Background = LinearGradient.Vertical(Colors.Gray5, Colors.Gray6);

		Content = new Grid
		{
			SystemInsetEdges = LayoutEdges.All,
			
			Rows =
			{
				GridLength.Star,
				GridLength.Auto
			},
			
			Children =
			{
				new StackPanel
				{
					VerticalAlignment = VerticalAlignment.Center,
					
					Children =
					{
						new Image
						{
							HorizontalAlignment = HorizontalAlignment.Center,
							Height = 130,
							Margin = new(0, -56, 0, 0),
							
							Source = ImageSource.Bundle("Splash.png")
						},
						
						new Label
						{
							Text = Texts.App_Name,
							TextAlignment = TextAlignment.Center,
							TextStyle = TextStyle.Title1,
							FontWeight = FontWeight.Bold,
							
							MaxLines = 1,
							Truncation = Truncation.Tail
						},
						new Label
						{
							Margin = new(0, 0, 0, 42),
							
							Text = Texts.App_Description,
							TextAlignment = TextAlignment.Center,
							TextStyle = TextStyle.Body,
						},
						
						new Button
						{
							Margin = new(0, 0, 0, 8),
							Padding = new(0, 12),
							
							Text = Texts.About_Terms,
							Kind = ButtonStyle.Tinted,
							
							Command = viewModel.ShowTermsCommand
						},
						new Button
						{
							Padding = new(0, 12),
							
							Text = Texts.About_Dependencies,
							Kind = ButtonStyle.Tinted,
							
							Command = viewModel.ShowDependenciesCommand
						}
					}
				},
				
				new StackPanel
				{
					Margin = new(0, 0, 0, 8),
					HorizontalAlignment = HorizontalAlignment.Center,
					Orientation = Orientation.Horizontal,
					Spacing = 8,
					
					Children =
					{
						new Label
						{
							Text = Texts.About_Version.Format(viewModel.Version),
							FontSize = 15,
							TextColor = Colors.SecondaryLabel
						},
						
						new Border
						{
							VerticalAlignment = VerticalAlignment.Center,
							Width = 3,
							Height = 3,
							
							Background = Colors.SecondaryLabel,
							CornerRadius = 1.5
						},
						
						new Button
						{
							Padding = Thickness.Zero,
							
							Text = Texts.About_Contact,
							Size = ButtonSize.Small,
							Command = viewModel.ContactCommand,
						}
					}
				}.Row(1)
			}
		};
	}
}