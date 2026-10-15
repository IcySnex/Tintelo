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
		Background = BindingFactory.Bind(MoodPaletteCatalog, catalog => catalog.Current)
			.ConvertTo<Color?>(current => Item is null
				? null
				: MoodPalette.Get(current, Item.Mood).Background);
		HighlightBackground = BindingFactory.Bind(MoodPaletteCatalog, catalog => catalog.Current)
			.ConvertTo<Color?>(current => Item is null
				? null
				: MoodPalette.Get(current, Item.Mood).Background.Map(
					light => light with
					{
						Red = Math.Clamp(light.Red * 0.95, 0, 1),
						Green = Math.Clamp(light.Green * 0.95, 0, 1),
						Blue = Math.Clamp(light.Blue * 0.95, 0, 1)
					},
					dark => dark with
					{
						Red = Math.Clamp(dark.Red * 0.85, 0, 1),
						Green = Math.Clamp(dark.Green * 0.85, 0, 1),
						Blue = Math.Clamp(dark.Blue * 0.85, 0, 1)
					}));

		CornerRadius = double.PositiveInfinity;

		Content = checkmark = new Image
		{
			Source = ImageSource.Symbol("checkmark", size: 22, weight: FontWeight.Bold),
			Tint = BindingFactory.Bind(MoodPaletteCatalog, catalog => catalog.Current)
				.ConvertTo<Color?>(current => Item is null
					? null
					: MoodPalette.Get(current, Item.Mood).Foreground),

			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
	}
	

	protected override void OnItemChanged(
		AddViewModel.MoodOption item) =>
		checkmark.IsVisible = item.IsSelected;
}
