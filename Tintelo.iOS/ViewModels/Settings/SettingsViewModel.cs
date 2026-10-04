using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Config;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.Services;
using Tintelo.iOS.Utils;
using Tintelo.iOS.ViewModels.About;

namespace Tintelo.iOS.ViewModels.Settings;

public partial class SettingsViewModel : ObservableObject
{
	readonly ILogger<SettingsViewModel> logger;
	readonly INavigator navigator;
	readonly Database database;
	readonly CalendarProvider calendar;

	public SettingsViewModel(
		ILogger<SettingsViewModel> logger,
		AppConfig config,
		INavigator navigator,
		Database database,
		CalendarProvider calendar)
	{
		this.logger = logger;
		this.navigator = navigator;
		this.database = database;
		this.calendar = calendar;
		
		Sections  =
		[
			new(Texts.Settings_Calendar,
			[
				new SettingsPickerEntry(
					Texts.Settings_Calendar_FirstDayOfWeek,
					OperatingSystem.IsIOSVersionAtLeast(26) ? "1.calendar" : "calendar",
					SettingsDisplays.FirstDayOfWeeks.Titles,
					BindingFactory.Bind(config, config => config.Calendar)
						.Path(calendar => calendar.FirstDayOfWeek)
						.ConvertTo(SettingsDisplays.FirstDayOfWeeks.GetTitle)
						.ConvertFrom(SettingsDisplays.FirstDayOfWeeks.GetValue)
						.TwoWay((calendar, value) => calendar.FirstDayOfWeek = value),
					Colors.SecondaryLabel),
	
				new SettingsTimeEntry(
					Texts.Settings_Calendar_StartOfDay,
					"moon.stars",
					BindingFactory.Bind(config, config => config.Calendar)
						.Path(calendar => calendar.StartOfDay)
						.ConvertTo(value => value.HasValue
							? TimePickerAnchor.Add(value.Value.ToTimeSpan())
							: TimePickerAnchor)
						.ConvertFrom(value => TimeOnly.FromDateTime(value))
						.TwoWay((calendar, value) => calendar.StartOfDay = value),
					TimePickerAnchor,
					TimePickerAnchor.AddHours(6).AddMinutes(-1))
			]),
			
			new(Texts.Settings_Theme,
			[
				new SettingsNavigationEntry(
					Texts.Settings_Theme_MoodPalette,
					"swatchpalette",
					typeof(SettingsMoodPaletteViewModel),
					BindingFactory.Bind(config, config => config.Theme)
						.Path(theme => theme.MoodPalette)
						.ConvertTo(SettingsDisplays.MoodPalettes.GetTitle)),
				
				new SettingsPickerEntry(
					Texts.Settings_Theme_Appearance,
					"moon",
					SettingsDisplays.Appearances.Titles,
					BindingFactory.Bind(config, config => config.Theme)
						.Path(theme => theme.Appearance)
						.ConvertTo(SettingsDisplays.Appearances.GetTitle)
						.ConvertFrom(SettingsDisplays.Appearances.GetValue)
						.TwoWay((theme, value) => theme.Appearance = value),
					Colors.SecondaryLabel),
				
				new SettingsPickerEntry(
					Texts.Settings_Theme_Accent,
					"paintbrush",
					SettingsDisplays.Accents.Titles,
					BindingFactory.Bind(config, config => config.Theme)
						.Path(theme => theme.Accent)
						.ConvertTo(SettingsDisplays.Accents.GetTitle)
						.ConvertFrom(SettingsDisplays.Accents.GetValue)
						.TwoWay((theme, value) => theme.Accent = value)),
		
				..OnIOSVersionAtLeast(27, new SettingsToggleEntry(
					Texts.Settings_Theme_SoftScrollEdge,
					"water.waves.and.arrow.trianglehead.down",
					BindingFactory.Bind(config, config => config.Theme)
						.Path(theme => theme.SoftScrollEdge)
						.TwoWay((theme, value) => theme.SoftScrollEdge = value)))
			]),
			
			new("Debug",
			[
				new SettingsActionEntry("Load Sample Data", "ladybug", LoadSampleDataCommand)
			])
		];
	}

	static readonly DateTime TimePickerAnchor = new(2001, 1, 1);
	
	static SettingsEntry[] OnIOSVersionAtLeast(
		int major,
		SettingsEntry entry) =>
		OperatingSystem.IsIOSVersionAtLeast(major)
			? [entry]
			: Array.Empty<SettingsEntry>();
	
	
	public IReadOnlyList<SettingsSection> Sections { get; }


	[RelayCommand]
	Task ShowAboutAsync() =>
		navigator.PushAsync<AboutViewModel>();

	[RelayCommand(AllowConcurrentExecutions = true)]
	async Task ActivateAsync(
		SettingsEntry entry)
	{
		switch (entry)
		{
			case SettingsActionEntry action when action.Command.CanExecute(null):
				await action.Command.ExecuteAsync(null);
				break;

			case SettingsNavigationEntry navigation:
				await navigator.PushAsync(navigation.ViewModelType);
				break;
		}
	}


	[RelayCommand]
	async Task LoadSampleDataAsync()
	{
		if (!await navigator.ConfirmAsync("Are you sure?", "This will erase all existing content in the database.", "Yes", "No", true))
			return;
		
		
		const int years = 1000;
		const int entryChance = 98;
		const int noteChance = 38;

		(string Name, string Emoji, string Color)[] sampleCategories =
		[
			("Work", "💼", "#3B82F6"),
			("Family", "🏠", "#F97316"),
			("Sport", "🏃", "#22C55E"),
			("Friends", "🍻", "#A855F7"),
			("Health", "🩺", "#EF4444"),
			("Travel", "✈️", "#06B6D4"),
			("Creative", "🎨", "#EC4899"),
			("Food", "🍜", "#EAB308")
		];

		string[] sampleNotes =
		[
			"Long walk by the lake.",
			"Coffee with Anna ☕",
			"Guter Tag im Büro.",
			"Finished the book I started in spring.",
			"Rain all day, stayed inside.",
			"Bouldering session after work.",
			"Family dinner at grandma's.",
			"Tired but happy.",
			"Planned the summer trip.",
			"Erste Tomaten aus dem Garten.",
			"Movie night with the kids 🍿",
			"Ran 10k without stopping!"
		];
		
		
		await database.WriteAsync((connection, transaction) =>
		{
			using (SqliteCommand clearEntries = connection.CreateCommand(transaction, "DELETE FROM Entry;"))
				clearEntries.ExecuteNonQuery();

			using (SqliteCommand clearCategories = connection.CreateCommand(transaction, "DELETE FROM Category;"))
				clearCategories.ExecuteNonQuery();

			List<Guid> categoryIds = [];
			using (SqliteCommand insertCategory = connection.CreateCommand(
				transaction,
				"INSERT INTO Category (Id, Name, Emoji, Color, IsArchived) VALUES ($id, $name, $emoji, $color, 0);"))
			{
				SqliteParameter id = insertCategory.Parameters.Add("$id", SqliteType.Text);
				SqliteParameter name = insertCategory.Parameters.Add("$name", SqliteType.Text);
				SqliteParameter emoji = insertCategory.Parameters.Add("$emoji", SqliteType.Text);
				SqliteParameter color = insertCategory.Parameters.Add("$color", SqliteType.Text);

				foreach ((string categoryName, string categoryEmoji, string categoryColor) in sampleCategories)
				{
					Guid categoryId = Guid.CreateVersion7();

					id.Value = categoryId.ToString("D");
					name.Value = categoryName;
					emoji.Value = categoryEmoji;
					color.Value = categoryColor;
					insertCategory.ExecuteNonQuery();

					categoryIds.Add(categoryId);
				}
			}

			DateOnly today = DateOnly.FromDateTime(DateTime.Now);
			DateOnly first = today.AddYears(-years);
			int entryCount = 0;

			using SqliteCommand insertEntry = connection.CreateCommand(
				transaction,
				"""
				INSERT INTO Entry (Id, Date, Mood, Note, CreatedUtc, UpdatedUtc)
				VALUES ($id, $date, $mood, $note, $created, $updated);
				""");
			SqliteParameter entryId = insertEntry.Parameters.Add("$id", SqliteType.Text);
			SqliteParameter entryDate = insertEntry.Parameters.Add("$date", SqliteType.Text);
			SqliteParameter entryMood = insertEntry.Parameters.Add("$mood", SqliteType.Integer);
			SqliteParameter entryNote = insertEntry.Parameters.Add("$note", SqliteType.Text);
			SqliteParameter created = insertEntry.Parameters.Add("$created", SqliteType.Text);
			SqliteParameter updated = insertEntry.Parameters.Add("$updated", SqliteType.Text);

			using SqliteCommand insertLink = connection.CreateCommand(
				transaction,
				"INSERT INTO EntryCategory (EntryId, CategoryId) VALUES ($entryId, $categoryId);");
			SqliteParameter linkEntry = insertLink.Parameters.Add("$entryId", SqliteType.Text);
			SqliteParameter linkCategory = insertLink.Parameters.Add("$categoryId", SqliteType.Text);

			Guid[] categoryPool = [.. categoryIds];

			for (DateOnly date = first; date <= today; date = date.AddDays(1))
			{
				if (Random.Shared.Next(100) >= entryChance)
					continue;

				Guid id = Guid.CreateVersion7();
				string? note = Random.Shared.Next(100) < noteChance
					? sampleNotes[Random.Shared.Next(sampleNotes.Length)]
					: null;
				DateTime timestamp = new DateTime(
					date.Year, date.Month, date.Day,
					Random.Shared.Next(7, 23), Random.Shared.Next(60), 0,
					DateTimeKind.Local).ToUniversalTime();
				string timestampText = SqliteValues.ToUtcText(timestamp);

				entryId.Value = id.ToString("D");
				entryDate.Value = SqliteValues.ToText(date);
				entryMood.Value = Random.Shared.Next(-3, 4);
				entryNote.Value = (object?)note ?? DBNull.Value;
				created.Value = timestampText;
				updated.Value = timestampText;
				insertEntry.ExecuteNonQuery();

				Random.Shared.Shuffle(categoryPool);
				int linkCount = Random.Shared.Next(4);
				linkEntry.Value = id.ToString("D");

				for (int index = 0; index < linkCount; index++)
				{
					linkCategory.Value = categoryPool[index].ToString("D");
					insertLink.ExecuteNonQuery();
				}

				entryCount++;
			}

			logger.LogInformation("Loaded {Entries} sample entries in {Categories} categories.", entryCount, categoryIds.Count);
			return true;
		});
		
		await calendar.ReloadAsync();
	}
}
