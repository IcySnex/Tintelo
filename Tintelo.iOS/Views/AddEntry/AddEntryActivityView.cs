using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;
using Tintelo.iOS.Utils.Extensions;
using Tintelo.iOS.ViewModels.AddEntry;

namespace Tintelo.iOS.Views.AddEntry;

[Page]
public class AddActivityView : ContentView<AddActivityViewModel>
{
	public AddActivityView(AddActivityViewModel viewModel) : base(viewModel)
	{
		Title = Texts.AddEntry_Activities;
		
		ToolbarItems.Add(new ToolbarGroup
		{
			Side = ToolbarSide.Leading,
			Items =
			{
				new ToolbarButton
				{
					Command = viewModel.PreviousCommand,
				
					Icon = ImageSource.Symbol("chevron.backward", weight: FontWeight.Semibold),
					Text = Texts.AddEntry_Previous,
				},
				new ToolbarButton
				{
					Command = viewModel.CloseCommand,
				
					Icon = ImageSource.Symbol("xmark", weight: FontWeight.Semibold),
					Text = Texts.AddEntry_Close,
				}
			}
		});
		ToolbarItems.Add(new ToolbarButton
		{
			Command = viewModel.ContinueCommand,

			Icon = ImageSource.Symbol("chevron.forward", weight: FontWeight.Semibold),
			Text = Texts.AddEntry_Continue,
			IsPrimary = true
		});
		
		StackPanel activities = new()
		{
			Padding = 16,
			Spacing = 12
		};

		foreach (Activity activity in viewModel.Activities)
		{
			activities.Children.Add(new StackPanel
			{
				Orientation = Orientation.Horizontal,
				Spacing = 12,

				Children =
				{
					new Image
					{
						Width = 20,
						Height = 20,

						Source = ImageSource.Symbol(
							activity.Icon?.ToSymbol() ?? "x.circle",
							size: 20,
							weight: FontWeight.Semibold,
							colors: activity.Color.ToColor()),
						Opacity = activity.Icon.HasValue ? 1 : 0.25
					},
					new Label
					{
						Text = activity.Name,
						VerticalAlignment = VerticalAlignment.Center
					}
				}
			});
		}

		Content = new ScrollView
		{
			Content = activities
		};
	}

	protected override void OnAppeared()
	{
		base.OnAppeared();

		UIResponder responder = Native;
		while (responder is not UINavigationController)
			responder = responder.NextResponder;

		if (responder is UINavigationController stack)
			SwipeBackSupport.Attach(stack);
	}
}
