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

	static readonly string[] DayNumbers = Enumerable.Range(0, 32)
		.Select(day => day.ToString(CultureInfo.CurrentCulture))
		.ToArray();
	
	static readonly MoodPaletteCatalog MoodPaletteCatalog = SkeleApplication.Current!.Services.GetRequiredService<MoodPaletteCatalog>();
	
	
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

		contentBorder.Background = BindingFactory.Bind(MoodPaletteCatalog, _ => ResolveBackground(), "palette => palette.Current");
		numberLabel.TextColor = BindingFactory.Bind(MoodPaletteCatalog, _ => ResolveForeground(), "palette => palette.Current");
		dotBorder.Background = BindingFactory.Bind(MoodPaletteCatalog, _ => ResolveDot(), "palette => palette.Current");
	}
	
	
	Color? ResolveBackground() =>
		Item is not CalendarDaySummary day ? null
		: day.Key > DateOnly.FromDateTime(DateTime.Now) ? Colors.Transparent
		: day.Mood is Mood mood ? MoodPalette.Get(MoodPaletteCatalog.Current, mood).Background
		: EmptyBackgroundColor;
	
	Color? ResolveForeground() =>
		Item is not CalendarDaySummary day ? null
		: day.Key > DateOnly.FromDateTime(DateTime.Now) ? Colors.SecondaryLabel.WithAlpha(0.6)
		: day.Mood is Mood mood ? MoodPalette.Get(MoodPaletteCatalog.Current, mood).Foreground
		: EmptyTextColor;
	
	Color ResolveDot() =>
		Item is CalendarDaySummary day && day.Mood is Mood mood
			? MoodPalette.Get(MoodPaletteCatalog.Current, mood).Foreground
			: EmptyTextColor;
	
	
	protected override void OnItemChanged(
		CalendarDaySummary item)
	{
		DateOnly today = DateOnly.FromDateTime(DateTime.Now);
		bool isFuture = item.Key > today;
		bool isToday = item.Key == today;
		bool isEmpty = !isFuture && item.Mood is null && !isToday;

		contentBorder.Stroke = isToday ? Colors.Label.WithAlpha(0.72) : isEmpty ? EmptyStrokeColor : default;
		contentBorder.StrokeThickness = isToday || isEmpty ? 2 : 0;
		contentBorder.StrokeDashPattern = isEmpty ? EmptyStrokeDashPattern : StrokeDashPattern;
		
		numberLabel.Text = DayNumbers[item.Key.Day];

		dotBorder.IsVisible = item.HasNote;
	}
}
