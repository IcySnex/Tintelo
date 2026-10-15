using System.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.ViewModels.Add;
using Tintelo.iOS.Views.Add.Cells;

namespace Tintelo.iOS.Views.Add;

[Page]
public class AddView : ContentView<AddViewModel>
{
	readonly ToolbarItem continueButton;
	
	public AddView(AddViewModel viewModel) : base(viewModel)
	{
		viewModel.PropertyChanged += OnViewModelPropertyChanged;

		Title = Bind(vm => vm.Title);

		BackButtonTitle = Texts.AddEntry_Previous;
		BackButtonStyle = BackButtonStyle.Minimal;
		
		ToolbarItems.Add(continueButton = new ToolbarItem
		{
			IsVisible = viewModel.SelectedMood.HasValue,
			Side = ToolbarSide.Trailing,

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
				continueButton.IsVisible = ViewModel.SelectedMood.HasValue;
				break;
		}
	}
}
