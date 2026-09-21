using SkeleKit;
using Tintelo.iOS.Localization;

namespace Tintelo.iOS.Views.Settings;

public class AboutHeader : Grid
{
	public AboutHeader()
	{
		Pressed = OnPressed;
		IsAccessibilityElement = true;
		AccessibilityLabel = Texts.App_Name;
		AccessibilityTraits = AccessibilityTrait.Button;

		Padding = 16;
		Background = Colors.SecondaryGroupedBackground;
		CornerRadius = SystemCornerRadius.GroupedList;
		CornerCurve = CornerCurve.Continuous;

		ColumnSpacing = 8;
		Columns.Add(60);
		Columns.Add(GridLength.Star);
		Columns.Add(10);
		
		Children.Add(new Image
		{
			Source = ImageSource.Bundle("Icon.png"),
			Width = 60,
			Height = 60
		});
		Children.Add(new StackPanel
		{
			VerticalAlignment = VerticalAlignment.Center,

			Children =
			{
				new Label
				{
					Text = Texts.App_Name,
					TextStyle = TextStyle.Title3,
					FontWeight = FontWeight.Bold
				},

				new Label
				{
					Text = Texts.App_Description,
					TextStyle = TextStyle.Subheadline,
					TextColor = Colors.SecondaryLabel,
					MaxLines = 2
				}
			}
		}.Column(1));
		Children.Add(new Image
		{
			Source = ImageSource.Symbol(
				"chevron.forward",
				weight: FontWeight.Semibold,
				colors: [Colors.Gray2])
		}.Column(2));
	}


	void OnPressed(
		bool pressed)
	{
		if (pressed)
			Background = Colors.Gray4;
		else
			Animate(0.3, () => Background = Colors.SecondaryGroupedBackground, layout: false);
	}
}
