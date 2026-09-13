using SkeleKit;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsActionEntryCell : SettingsEntryCell<SettingsActionEntry>
{
	public SettingsActionEntryCell()
	{
		IsEnabled = Bind(item => item.Command)
			.Path(command => command.IsRunning)
			.ConvertTo(running => !running);

		Tint = Bind(item => item.Color);
		TextView.TextColor = Bind(item => item.Color);
		IconView.Tint = default;
		
		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add(new ActivityIndicator
		{
			IsAnimating = Bind(item => item.Command)
				.Path(command => command.IsRunning)
		}.Column(2));
	}
}
