using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings;

public class SettingsHeaderView : ItemView<SettingsSection>
{
	public SettingsHeaderView() =>
		Content = new Grid
		{
			Padding = 16,
			Background = Color.Dynamic(Colors.Background, Colors.SecondaryBackground),
			CornerRadius = SystemCornerRadius.GroupedList,
			CornerCurve = CornerCurve.Continuous,

			ColumnSpacing = 8,
			Columns =
			{
				60,
				GridLength.Star,
				10
			},

			Children =
			{
				new Image
				{
					Source = ImageSource.Bundle("Icon.png"),
					Width = 60,
					Height = 60
				},

				new StackPanel
				{
					VerticalAlignment = VerticalAlignment.Center,

					Children =
					{
						new Label
						{
							Text = Texts.App_Name,
							FontSize = 20,
							FontWeight = FontWeight.Semibold
						},

						new Label
						{
							Text = Texts.App_Description,
							FontSize = 15,
							TextColor = Colors.Gray,
							MaxLines = 2
						}
					}
				}.Column(1),

				new Image
				{
					Source = ImageSource.Symbol("chevron.forward"),
					SymbolColors = { Colors.Gray4 },
					SymbolWeight = FontWeight.Bold
				}.Column(2)
			}
		};
}
