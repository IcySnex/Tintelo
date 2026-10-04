using Microsoft.Extensions.DependencyInjection;
using SkeleKit;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Settings;

namespace Tintelo.iOS.Views.Settings.Cells;

public sealed class SettingsActionEntryCell : SettingsEntryCell<SettingsActionEntry>
{
	static readonly AppConfig Config = SkeleApplication.Current!.Services.GetRequiredService<AppConfig>();


	public SettingsActionEntryCell()
	{
		IsEnabled = Bind(item => item.Command)
			.Path(command => command.IsRunning)
			.ConvertTo(running => !running);

		TextView.TextColor = BindingFactory.Bind(Config.Theme, _ => ResolveColor(), "theme => theme.Accent");
		IconView.Tint = BindingFactory.Bind(Config.Theme, _ => ResolveColor(), "theme => theme.Accent");
		
		ContainerView.Columns.Add(GridLength.Auto);
		ContainerView.Children.Add(new ActivityIndicator
		{
			IsAnimating = Bind(item => item.Command)
				.Path(command => command.IsRunning)
		}.Column(2));
	}

	
	Color? ResolveColor() =>
		Item?.Color ?? SkeleApplication.Current?.Theme.Tint;
}
