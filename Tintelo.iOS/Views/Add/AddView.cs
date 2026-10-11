using SkeleKit;
using Tintelo.iOS.ViewModels.Add;
using Tintelo.iOS.Views.Add.Cells;

namespace Tintelo.iOS.Views.Add;

[Page]
public class AddView : ContentView<AddViewModel>
{
	public AddView(AddViewModel viewModel) : base(viewModel)
	{
		Title = Bind(vm => vm.Title);

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
}
