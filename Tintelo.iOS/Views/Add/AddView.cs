using System.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels.Add;
using Tintelo.iOS.Views.Add.Cells;

namespace Tintelo.iOS.Views.Add;

[Page]
public class AddView : ContentView<AddViewModel>
{
	readonly ToolbarItem closeButton;
	readonly ToolbarItem continueButton;
	
	public AddView(AddViewModel viewModel) : base(viewModel)
	{
		viewModel.PropertyChanged += OnViewModelPropertyChanged;

		Title = Bind(vm => vm.Title);

		BackButtonTitle = Texts.AddEntry_Previous;
		BackButtonStyle = BackButtonStyle.Minimal;

		ToolbarItems.Add(closeButton = new ToolbarItem
		{
			IsVisible = viewModel.SelectedMood.HasValue,
			Side = ToolbarSide.Leading,
			
			Command = viewModel.CloseCommand,
			
			Icon = ImageSource.Symbol("xmark", weight: FontWeight.Semibold),
			Text = Texts.AddEntry_Close,
			Tint = Colors.Label
		});
		ToolbarItems.Add(continueButton = new ToolbarItem
		{
			IsVisible = viewModel.SelectedMood.HasValue,

			Command = viewModel.ContinueCommand,

			Icon = ImageSource.Symbol("chevron.forward", weight: FontWeight.Semibold),
			Text = Texts.AddEntry_Continue,
			IsPrimary = true
		});

		MinimumContentInsets = new Thickness(0, 0, 0, 36);
		Content = new CollectionView<AddViewModel.MoodOption>
		{
			MaxWidth = 500,
			SizesToContent = true,

			Layout = CollectionLayout.Grid(
				columns: 7,
				spacing: 12,
				itemAspectRatio: 0.5,
				outerSpacing: new Thickness(12, 0)),

			ShowsIndicator = false,
			Bounces = false,
			RetainsHighlight = false,

			ItemTemplate = static () => new MoodOptionCell(),

			ItemsSource = viewModel.Moods,
			ItemCommand = viewModel.SelectMoodCommand
		};
		
	}

	
	void OnViewModelPropertyChanged(object? _, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(AddViewModel.SelectedMood):
				if (ViewModel.SelectedMood.HasValue)
				{
					closeButton.IsVisible = true;
					continueButton.IsVisible = true;
					ConfirmLeave = () => Navigator.ConfirmAsync(
						Texts.AddEntry_ConfirmLeave,
						Texts.AddEntry_ConfirmLeave_Description,
						Texts.AddEntry_ConfirmLeave_Yes,
						Texts.AddEntry_ConfirmLeave_No,
						true);
				}
				else
				{
					closeButton.IsVisible = false;
					continueButton.IsVisible = false;
					ConfirmLeave = null;
				}
				break;
		}
	}
		
}
