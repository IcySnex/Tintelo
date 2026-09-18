using SkeleKit;
using Tintelo.iOS.ViewModels;

namespace Tintelo.iOS.Views;

[Page]
public sealed class CalendarDayDetailView : ContentView<CalendarViewModel>
{
	public CalendarDayDetailView(
		CalendarViewModel viewModel) : base(viewModel)
	{
		Title = Bind(vm => vm.SelectedNavigationTitle);
		// NavigationTitleStyle = TitleStyle.Inline;
		Background = Colors.Background.WithAlpha(0.5);

		Content = new ScrollView
		{
			Content = new StackPanel
			{
				Padding = new(22, 24, 22, 40),
				Spacing = 20,

				Children =
				{
					new StackPanel
					{
						Spacing = 2,

						Children =
						{
							new Label
							{
								Text = Bind(vm => vm.SelectedWeekday),
								TextStyle = TextStyle.Title1,
								FontWeight = FontWeight.Bold,
								MaxLines = 1
							},
							new Label
							{
								Text = Bind(vm => vm.SelectedDateTitle),
								TextStyle = TextStyle.Subheadline,
								TextColor = Colors.SecondaryLabel,
								MaxLines = 1
							}
						}
					},

					EntryContent(),
					EmptyContent()
				}
			}
		};
	}


	static View EntryContent() =>
		new StackPanel
		{
			IsVisible = Bind(vm => vm.HasSelectedEntry),
			Spacing = 20,

			Children =
			{
				Section(
					"MOOD",
					new Border
					{
						Background = Colors.SecondaryGroupedBackground,
						CornerRadius = 18,
						CornerCurve = CornerCurve.Continuous,
						Padding = 18,

						Child = new StackPanel
						{
							Orientation = Orientation.Horizontal,
							Spacing = 14,

							Children =
							{
								new Image
								{
									VerticalAlignment = VerticalAlignment.Center,
									Width = 42,
									Height = 42,
									Source = ImageSource.Symbol("circle.fill"),
									Tint = Bind(vm => vm.SelectedMoodColor)
								},
								new StackPanel
								{
									VerticalAlignment = VerticalAlignment.Center,
									Spacing = 2,

									Children =
									{
										new Label
										{
											Text = Bind(vm => vm.SelectedMoodName),
											TextStyle = TextStyle.Title3,
											FontWeight = FontWeight.Semibold,
											MaxLines = 1
										},
										new Label
										{
											Text = "How this day felt",
											TextStyle = TextStyle.Footnote,
											TextColor = Colors.SecondaryLabel,
											MaxLines = 1
										}
									}
								}
							}
						}
					}),

				Section(
					"CATEGORIES",
					new StackPanel
					{
						Orientation = Orientation.Horizontal,
						Spacing = 8,

						Children =
						{
							Chip("🌿", "Outside"),
							Chip("☕️", "Friends")
						}
					}),

				new StackPanel
				{
					IsVisible = Bind(vm => vm.HasSelectedNote),
					Spacing = 8,

					Children =
					{
						SectionTitle("NOTE"),
						new Border
						{
							Background = Colors.SecondaryGroupedBackground,
							CornerRadius = 18,
							CornerCurve = CornerCurve.Continuous,
							Padding = 18,

							Child = new Label
							{
								Text = "Took the long way home after coffee. The evening was quiet, warm, and exactly what I needed.",
								TextStyle = TextStyle.Body
							}
						}
					}
				}
			}
		};

	static View EmptyContent() =>
		new Border
		{
			IsVisible = Bind(vm => vm.HasNoSelectedEntry),
			Background = Colors.SecondaryGroupedBackground,
			CornerRadius = 18,
			CornerCurve = CornerCurve.Continuous,
			Padding = new(24, 30),

			Child = new StackPanel
			{
				Spacing = 10,

				Children =
				{
					new Image
					{
						HorizontalAlignment = HorizontalAlignment.Center,
						Width = 34,
						Height = 34,
						Source = ImageSource.Symbol("calendar.badge.plus"),
						Tint = Colors.SecondaryLabel
					},
					new Label
					{
						Text = "Nothing recorded",
						TextAlignment = TextAlignment.Center,
						TextStyle = TextStyle.Title3,
						FontWeight = FontWeight.Semibold
					},
					new Label
					{
						Text = "This day is still open. When you add an entry, its mood, categories, and note will appear here.",
						TextAlignment = TextAlignment.Center,
						TextStyle = TextStyle.Subheadline,
						TextColor = Colors.SecondaryLabel
					}
				}
			}
		};

	static View Section(
		string title,
		View content) =>
		new StackPanel
		{
			Spacing = 8,
			Children =
			{
				SectionTitle(title),
				content
			}
		};

	static Label SectionTitle(
		string title) =>
		new()
		{
			Margin = new(4, 0),
			Text = title,
			TextStyle = TextStyle.Caption1,
			FontWeight = FontWeight.Semibold,
			TextColor = Colors.SecondaryLabel,
			MaxLines = 1
		};

	static Border Chip(
		string emoji,
		string title) =>
		new()
		{
			Background = Colors.TertiaryGroupedBackground,
			CornerRadius = 14,
			CornerCurve = CornerCurve.Continuous,
			Padding = new(12, 7),

			Child = new Label
			{
				Text = $"{emoji}  {title}",
				TextStyle = TextStyle.Subheadline,
				FontWeight = FontWeight.Medium,
				MaxLines = 1
			}
		};
}
