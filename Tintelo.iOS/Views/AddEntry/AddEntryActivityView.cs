using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;
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
			activities.Children.Add(new Label
			{
				Text = activity.Emoji is null
					? activity.Name
					: $"{activity.Emoji} {activity.Name}"
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
