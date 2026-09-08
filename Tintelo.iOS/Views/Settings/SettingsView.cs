using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels.Settings;

namespace Tintelo.iOS.Views.Settings;

[Page]
public class SettingsView : ContentView<SettingsViewModel>
{
	public SettingsView(SettingsViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Settings_Title;

		Content = new ScrollView()
		{
			Content = new StackPanel()
			{
				Children =
				{
					new Label
					{
						Text = "hi",
					}
				}
			}
		};
	}
}