using Microsoft.Extensions.DependencyInjection;
using SkeleKit;
using Tintelo.iOS.Models.Palette;
using Tintelo.iOS.Services;
using Tintelo.iOS.ViewModels.Add;

namespace Tintelo.iOS.Views.Add.Cells;

public class MoodOptionCell : ItemView<AddViewModel.MoodOption>
{
	static readonly MoodPaletteCatalog MoodPaletteCatalog = SkeleApplication.Current!.Services.GetRequiredService<MoodPaletteCatalog>();


	readonly Image checkmark;
	
	public MoodOptionCell()
	{
		HighlightBackground = null;

		Content = new Border
		{
			CornerRadius = double.PositiveInfinity,

			Background = BindingFactory.Bind(MoodPaletteCatalog, catalog => catalog.Current)
				.ConvertTo<Color?>(current => Item is null 
					? null
					: MoodPalette.Get(current, Item.Mood).Background),

			Child = checkmark = new Image
			{
				Source = ImageSource.Symbol("checkmark", size: 22, weight: FontWeight.Bold),
				Tint = BindingFactory.Bind(MoodPaletteCatalog, catalog => catalog.Current)
					.ConvertTo<Color?>(current => Item is null 
						? null
						: MoodPalette.Get(current, Item.Mood).Foreground),

				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			}
		};
	}
	

	protected override void OnItemChanged(
		AddViewModel.MoodOption item) =>
		checkmark.IsVisible = item.IsSelected;
}
