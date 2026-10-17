using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Journal;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.AddEntry;

namespace Tintelo.iOS.Views.AddEntry;

[Page]
public class AddCategoryView : ContentView<AddCategoryViewModel>
{
	public AddCategoryView(AddCategoryViewModel viewModel) : base(viewModel)
	{
		Title = Texts.AddEntry_Categories;
		
		ToolbarItems.Add(new ToolbarItem
		{
			Side = ToolbarSide.Leading,
			
			Command = viewModel.PreviousCommand,
			
			Icon = ImageSource.Symbol("chevron.backward", weight: FontWeight.Semibold),
			Text = Texts.AddEntry_Previous,
			Tint = Colors.ToolbarButton
		});
		ToolbarItems.Add(new ToolbarItem
		{
			Side = ToolbarSide.Leading,
			
			Command = viewModel.CloseCommand,
			
			Icon = ImageSource.Symbol("xmark", weight: FontWeight.Semibold),
			Text = Texts.AddEntry_Close,
			Tint = Colors.ToolbarButton
		});
		
		ToolbarItems.Add(new ToolbarItem
		{
			Command = viewModel.ContinueCommand,

			Icon = ImageSource.Symbol("chevron.forward", weight: FontWeight.Semibold),
			Text = Texts.AddEntry_Continue,
			IsPrimary = true
		});
		
		StackPanel categories = new()
		{
			Padding = 16,
			Spacing = 12
		};

		foreach (Category category in viewModel.Categories)
		{
			categories.Children.Add(new Label
			{
				Text = category.Emoji is null
					? category.Name
					: $"{category.Emoji} {category.Name}"
			});
		}

		Content = new ScrollView
		{
			Content = categories
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
