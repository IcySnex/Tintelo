using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SkeleKit;
using Tintelo.iOS.Models;
using Tintelo.iOS.Models.Calendar;
using Tintelo.iOS.Models.Palette;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Views.Calendar.Cells;

public class CalendarDayCell: ItemView<CalendarDaySummary>
{
	const double MaxVisualSize = 52;
	
	static readonly Color EmptyBackgroundColor = Color.Dynamic(Color.FromHex(0xf5f5f5), Color.FromHex(0x262626));
	static readonly Color EmptyStrokeColor = Color.Dynamic(Color.FromHex(0xdbdbdb), Color.FromHex(0x636363));
	static readonly Color EmptyTextColor = Color.Dynamic(Color.FromHex(0x646464), Color.FromHex(0xdadada));

	static readonly double[] EmptyStrokeDashPattern = [1, 5];
	static readonly double[] StrokeDashPattern = [];

	
	static readonly MoodPaletteCatalog MoodPaletteCatalog = SkeleApplication.Current!.Services.GetRequiredService<MoodPaletteCatalog>();
	
	static BindableBrush ResolvePaletteBackground(
		Mood mood) =>
		BindingFactory.Bind(MoodPaletteCatalog, moodPalette => moodPalette.Current)
			.ConvertTo(value => MoodPalette.Get(value, mood).Background);
	
	static BindingExpression<Color?> ResolvePaletteForeground(
		Mood mood) =>
		BindingFactory.Bind(MoodPaletteCatalog, moodPalette => moodPalette.Current)
			.ConvertTo(value => (Color?)MoodPalette.Get(value, mood).Foreground);
	
	
	readonly Border contentBorder;
	readonly Label numberLabel;
	readonly Border dotBorder;
	
	public CalendarDayCell()
	{
		MaxHeight = 60;
		MaxWidth = 60;
		HighlightBackground = null;

		Content = contentBorder = new Border
		{
			MaxWidth = MaxVisualSize,
			MaxHeight = MaxVisualSize,
			Margin = 2,
			
			CornerRadius = MaxVisualSize / 2,
			StrokeLineCap = StrokeLineCap.Round,

			Child = new Overlay
			{
				Children =
				{
					(numberLabel = new Label
					{
						HorizontalAlignment = HorizontalAlignment.Stretch,

						MaxLines = 1,

						TextStyle = TextStyle.Callout,
						FontWeight = FontWeight.Semibold,
						TextAlignment = TextAlignment.Center,

						MaxFontSize = 24,
						AutoShrink = 0.7
					}),

					(dotBorder = new Border
					{
						Width = 4,
						Height = 4,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.End,
						Margin = new Thickness(0, 0, 0, 6),
					
						CornerRadius = 2
					})
				}
			}
		};
	}
	
	
	protected override void OnItemChanged(
		CalendarDaySummary item)
	{
		bool isFuture = item.Day > DateTime.Now.Day;
		bool isToday = item.Day == DateTime.Now.Day;
		bool isEmpty = !isFuture && item.Mood is null && !isToday;

		contentBorder.Background = isFuture ?Colors.Transparent : item.Mood.HasValue ? ResolvePaletteBackground(item.Mood.Value) : EmptyBackgroundColor;
		contentBorder.Stroke = isToday ? Colors.Label.WithAlpha(0.72) : isEmpty ? EmptyStrokeColor : default;
		contentBorder.StrokeThickness = isToday || isEmpty ? 2 : 0;
		contentBorder.StrokeDashPattern = isEmpty ? EmptyStrokeDashPattern : StrokeDashPattern;
		
		numberLabel.Text = item.Day.ToString(CultureInfo.CurrentCulture);
		numberLabel.TextColor = isFuture ? Colors.SecondaryLabel.WithAlpha(0.6) : item.Mood.HasValue ? ResolvePaletteForeground(item.Mood.Value) : EmptyTextColor;

		dotBorder.IsVisible = item.HasNote;
		dotBorder.Background = item.Mood.HasValue ? ResolvePaletteForeground(item.Mood.Value) : EmptyTextColor;
	}
}
