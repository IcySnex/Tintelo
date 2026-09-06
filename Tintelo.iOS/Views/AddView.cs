using SkeleKit;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public class AddView : ContentView<AddViewModel>
{
	public AddView(AddViewModel viewModel) : base(viewModel)
	{
		Title = "Add Entry";
	}
}