using System.Windows.Input;
using SkeleKit;

namespace Tintelo.iOS.Views;

public class ConfirmView : ContentView
{
	public ConfirmView(
		string message,
		string accept,
		bool isDestructive,
		ICommand command,
		object? commandParameter = null)
	{
		Background = Colors.SecondaryGroupedBackground;
		SafeAreaEdges = SafeAreaEdges.None;
		HidesNavigationBar = true;

		Content = new StackPanel
		{
			Padding = new Thickness(20),
			Spacing = 20,
			
			VerticalAlignment = VerticalAlignment.Start,
			HorizontalAlignment = HorizontalAlignment.Center,

			Children =
			{
				new Label
				{
					Text = message,
					TextStyle = TextStyle.Body,
					TextAlignment = TextAlignment.Leading
				},

				new Button
				{
					Text = accept,
					
					IsDestructive = isDestructive,
					Kind = ButtonStyle.Gray,
					Size = ButtonSize.Large,
					
					Command = command,
					CommandParameter = commandParameter
				}
			}
		};
	}
}