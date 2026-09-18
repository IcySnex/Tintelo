using CoreAnimation;
using System.Globalization;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public class CalendarView : ContentView<CalendarViewModel>
{
	public CalendarView(CalendarViewModel viewModel) : base(viewModel)
	{
		Title = Texts.Calendar_Title;
		
		Background = Colors.Background;
		NavigationAccessory = new CalendarWeekdayHeader();

		ToolbarItems.Add(new()
		{
			Icon = "switch.2",
			Text = Texts.Settings_Title,
			Command = viewModel.OpenSettingsCommand
		});

		Content = new CollectionView<CalendarDayPreview, CalendarMonthPreview>
		{
			GroupedItemsSource = CalendarPreview.Months,
			ItemTemplate = static () => new CalendarDayCell(),
			SectionHeaderTemplate = static () => new CalendarMonthHeaderCell(),
			ItemCommand = Command.From<CalendarDayPreview>(item =>
			{
				if (item is { Date: DateTime date, IsFuture: false })
					viewModel.SelectDay(date, item.Mood?.Name, item.Mood?.Background, item.HasNote);
			}),
			Layout = CollectionLayout.Grid(columns: 7, spacing: 6),
			Padding = new(10, 0, 10, 32),
			RetainsSelection = false,
			ShowsSeparators = false
		};
	}
}

internal sealed class CalendarWeekdayHeader : Border
{
	public CalendarWeekdayHeader()
	{
		Grid weekdays = new()
		{
			Margin = new(16, 8),
			ColumnSpacing = 6
		};

		for (int column = 0; column < 7; column++)
			weekdays.Columns.Add(GridLength.Star);

		for (int column = 0; column < CalendarPreview.WeekdayTitles.Length; column++)
		{
			weekdays.Children.Add(new Label
			{
				Text = CalendarPreview.WeekdayTitles[column],
				TextStyle = TextStyle.Caption1,
				MaxFontSize = 19,
				AutoShrink = 0.7,
				FontWeight = FontWeight.Semibold,
				TextColor = Colors.SecondaryLabel,
				TextAlignment = TextAlignment.Center,
				MaxLines = 1
			}.Column(column));
		}

		Child = weekdays;
	}
}

internal sealed class CalendarMonthHeaderCell : ItemView<CalendarMonthPreview>
{
	public CalendarMonthHeaderCell()
	{
		HighlightBackground = null;

		Content = new StackPanel
		{
			Orientation = Orientation.Horizontal,
			Margin = new(0, 12, 0, 4),
			Spacing = 7,

			Children =
			{
				new Label
				{
					Text = Bind(month => month.Month),
					TextStyle = TextStyle.Title2,
					FontWeight = FontWeight.Semibold,
					MaxLines = 1
				},
				new Label
				{
					VerticalAlignment = VerticalAlignment.End,
					Text = Bind(month => month.Year),
					TextStyle = TextStyle.Subheadline,
					FontWeight = FontWeight.Semibold,
					TextColor = Colors.SecondaryLabel,
					MaxLines = 1
				}
			}
		};
	}
}

internal sealed class CalendarDayCell : ItemView<CalendarDayPreview>
{
	readonly CalendarDayCircle circle;
	readonly Label number;
	readonly Border noteMarker;


	public CalendarDayCell()
	{
		HighlightBackground = null;

		number = new()
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			TextStyle = TextStyle.Callout,
			MaxFontSize = 24,
			AutoShrink = 0.7,
			FontWeight = FontWeight.Semibold,
			TextAlignment = TextAlignment.Center,
			MaxLines = 1
		};

		noteMarker = new()
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.End,
			Margin = new(0, 0, 0, 6),
			Width = 4,
			Height = 4,
			CornerRadius = 2
		};

		circle = new()
		{
			Child = new Grid
			{
				Children =
				{
					number,
					noteMarker
				}
			}
		};

		Content = circle;
	}


	protected override void OnItemChanged(
		CalendarDayPreview? item)
	{
		base.OnItemChanged(item);

		if (item is null || item.Date is not DateTime date)
		{
			Content!.IsVisible = false;
			IsAccessibilityElement = false;
			AccessibilityLabel = string.Empty;
			return;
		}

		Content!.IsVisible = true;
		number.Text = date.Day.ToString(CultureInfo.CurrentCulture);
		number.TextColor = item.IsFuture
			? Colors.SecondaryLabel.WithAlpha(0.6)
			: item.Mood?.Text ?? CalendarPalette.EmptyText;
		number.Opacity = 1;
		noteMarker.IsVisible = item.HasNote;
		noteMarker.Background = item.Mood?.Text ?? CalendarPalette.EmptyText;
		Background = null;
		CornerRadius = 12;
		CornerCurve = CornerCurve.Continuous;

		circle.Background = item.IsFuture
			? Colors.Transparent
			: item.Mood?.Background ?? CalendarPalette.EmptyBackground;
		circle.ShowsDottedStroke = !item.IsFuture
			&& item.Mood is null
			&& !item.IsToday;
		circle.Stroke = item.IsToday
			? Colors.Label.WithAlpha(0.72)
			: null;
		circle.StrokeThickness = item.IsToday ? 2 : 0;

		IsAccessibilityElement = true;
		AccessibilityLabel = item.AccessibilityLabel;
	}

	protected override Size MeasureOverride(
		Size availableSize)
	{
		double width = double.IsFinite(availableSize.Width)
			? availableSize.Width
			: CalendarLayoutMetrics.DayMaxRowHeight;
		double height = Math.Min(width, CalendarLayoutMetrics.DayMaxRowHeight);
		double marker = MarkerSize(new(width, height));

		Content?.Measure(new(marker, marker));
		return new(width, height);
	}

	protected override Size ArrangeOverride(
		Size finalSize)
	{
		double marker = MarkerSize(finalSize);
		Content?.Arrange(new(
			(finalSize.Width - marker) / 2,
			(finalSize.Height - marker) / 2,
			marker,
			marker));

		return finalSize;
	}

	static double MarkerSize(
		Size available) =>
		Math.Min(
			CalendarLayoutMetrics.DayMaxVisualSize,
			Math.Max(0, Math.Min(available.Width, available.Height) - 4));
}

internal static class CalendarLayoutMetrics
{
	public const double DayMaxRowHeight = 60;
	public const double DayMaxVisualSize = 52;
}

internal sealed class CalendarDayCircle : Border
{
	readonly CAShapeLayer dottedStroke;
	CGSize dottedStrokeSize;


	public CalendarDayCircle()
	{
		dottedStroke = new()
		{
			FillColor = UIColor.Clear.CGColor,
			LineWidth = 2,
			LineCap = CAShapeLayer.CapRound,
			LineDashPattern = [NSNumber.FromInt32(1), NSNumber.FromInt32(5)],
			Hidden = true
		};

		Native.Layer.AddSublayer(dottedStroke);
	}


	public bool ShowsDottedStroke
	{
		get;
		set
		{
			field = value;
			dottedStroke.Hidden = !value;
		}
	}


	protected override Size ArrangeOverride(
		Size finalSize)
	{
		CornerRadius = Math.Min(finalSize.Width, finalSize.Height) / 2;
		Size result = base.ArrangeOverride(finalSize);
		nfloat width = (nfloat)finalSize.Width;
		nfloat height = (nfloat)finalSize.Height;

		if (dottedStrokeSize.Width != width || dottedStrokeSize.Height != height)
		{
			dottedStrokeSize = new(width, height);
			dottedStroke.Frame = new(0, 0, width, height);
			dottedStroke.Path = UIBezierPath.FromOval(
				new CGRect(1, 1, Math.Max(0, width - 2), Math.Max(0, height - 2))).CGPath;
		}
		dottedStroke.StrokeColor = Native.TraitCollection.UserInterfaceStyle is UIUserInterfaceStyle.Dark
			? UIColor.FromRGB(0x63, 0x63, 0x63).CGColor
			: UIColor.FromRGB(0xdb, 0xdb, 0xdb).CGColor;

		return result;
	}
}

internal sealed record CalendarMoodPreview(
	Color Background,
	Color Text,
	string Name);

internal sealed record CalendarDayPreview(
	DateTime? Date,
	string AccessibilityLabel = "",
	CalendarMoodPreview? Mood = null,
	bool IsToday = false,
	bool IsFuture = false,
	bool HasNote = false);

internal sealed record CalendarMonthPreview(
	string Month,
	string Year,
	IReadOnlyList<CalendarDayPreview> Items) : ISection<CalendarDayPreview>;

internal static class CalendarPalette
{
	public static readonly Color EmptyBackground = Color.Dynamic(
		Color.FromHex(0xf5f5f5),
		Color.FromHex(0x262626));

	public static readonly Color EmptyText = Color.Dynamic(
		Color.FromHex(0x646464),
		Color.FromHex(0xdadada));

	public static readonly CalendarMoodPreview ExtremelyGood = new(
		Color.Dynamic(Color.FromHex(0x187d68), Color.FromHex(0x23866f)),
		Colors.White,
		"Extremely good");

	public static readonly CalendarMoodPreview VeryGood = new(
		Color.Dynamic(Color.FromHex(0x4dab86), Color.FromHex(0x4fac88)),
		Color.FromHex(0x163f31),
		"Very good");

	public static readonly CalendarMoodPreview Good = new(
		Color.Dynamic(Color.FromHex(0xa7d3bc), Color.FromHex(0x8fc3a8)),
		Color.FromHex(0x315947),
		"Good");

	public static readonly CalendarMoodPreview Neutral = new(
		Color.Dynamic(Color.FromHex(0xd8d5cf), Color.FromHex(0x494844)),
		Color.Dynamic(Color.FromHex(0x5d5a54), Color.FromHex(0xe6e3dd)),
		"Neutral");

	public static readonly CalendarMoodPreview Bad = new(
		Color.Dynamic(Color.FromHex(0xead2b1), Color.FromHex(0xc7aa88)),
		Color.FromHex(0x675137),
		"Bad");

	public static readonly CalendarMoodPreview VeryBad = new(
		Color.Dynamic(Color.FromHex(0xe29a62), Color.FromHex(0xd28d5c)),
		Color.FromHex(0x603919),
		"Very bad");

	public static readonly CalendarMoodPreview ExtremelyBad = new(
		Color.Dynamic(Color.FromHex(0xca555a), Color.FromHex(0xc9585d)),
		Colors.White,
		"Extremely bad");
}

internal static class CalendarPreview
{
	static readonly CultureInfo Culture = CultureInfo.CurrentCulture;

	static readonly IReadOnlyDictionary<int, CalendarMoodPreview> RecordedDays =
		new Dictionary<int, CalendarMoodPreview>
		{
			[1] = CalendarPalette.Neutral,
			[2] = CalendarPalette.ExtremelyBad,
			[3] = CalendarPalette.VeryBad,
			[5] = CalendarPalette.Bad,
			[6] = CalendarPalette.Neutral,
			[7] = CalendarPalette.ExtremelyGood,
			[8] = CalendarPalette.VeryGood,
			[10] = CalendarPalette.Bad,
			[12] = CalendarPalette.VeryGood
		};


	public static string[] WeekdayTitles { get; } = CreateWeekdayTitles();

	public static CalendarMonthPreview[] Months { get; } =
	[
		September2026()
	];


	static string[] CreateWeekdayTitles()
	{
		string[] names = Culture.DateTimeFormat.ShortestDayNames;
		int first = (int)Culture.DateTimeFormat.FirstDayOfWeek;
		string[] ordered = new string[7];

		for (int index = 0; index < ordered.Length; index++)
			ordered[index] = names[(first + index) % 7].ToUpper(Culture);

		return ordered;
	}

	static CalendarMonthPreview September2026()
	{
		const int year = 2026;
		const int month = 9;
		DateTime monthStart = new(year, month, 1);
		int first = (int)Culture.DateTimeFormat.FirstDayOfWeek;
		int leadingDays = ((int)monthStart.DayOfWeek - first + 7) % 7;
		List<CalendarDayPreview> days = [];

		for (int index = 0; index < leadingDays; index++)
			days.Add(new(null));

		for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
		{
			DateTime date = new(year, month, day);
			RecordedDays.TryGetValue(day, out CalendarMoodPreview? mood);
			days.Add(new(
				date,
				date.ToString("D", Culture),
				mood,
				IsToday: day == 13,
				IsFuture: day > 13,
				HasNote: day is 7 or 12));
		}

		return new(
			Culture.DateTimeFormat.GetMonthName(month),
			year.ToString(Culture),
			days);
	}
}
