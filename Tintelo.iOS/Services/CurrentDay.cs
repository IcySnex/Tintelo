using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Config;

namespace Tintelo.iOS.Services;

public partial class CurrentDay : ObservableObject
{
	static readonly TimeSpan MaxTimerDelay = TimeSpan.FromMinutes(5);

	readonly ILogger<CurrentDay> logger;
	readonly AppConfig config;
	readonly Lock gate = new();

	Timer? timer;
	DateOnly? appliedIconDay;
	
	public CurrentDay(
		ILogger<CurrentDay> logger,
		AppConfig config)
	{
		this.config = config;
		this.logger = logger;

		config.Calendar.PropertyChanged += OnConfigurationChanged;

		EventHandler<NSNotificationEventArgs> handler = (_, e) =>
		{
			logger.LogInformation("System sent relevant clock event: {name}", e.Notification.Name);
			Refresh();
		};
		UIApplication.Notifications.ObserveSignificantTimeChange(handler);
		NSTimeZone.Notifications.ObserveSystemTimeZoneDidChange(handler);
		NSDate.Notifications.ObserveSystemClockDidChange(handler);

		Apply();
	}
	
	
	void OnConfigurationChanged(
		object? sender,
		PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(config.Calendar.StartOfDay):
				Refresh();
				break;
		}
	}


	UITab? CalendarTab
	{
		get
		{
			if (field is not null)
				return field;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			if (UIApplication.SharedApplication is null)
				return null;

			foreach (UIWindowScene scene in UIApplication.SharedApplication.ConnectedScenes.OfType<UIWindowScene>())
			{
				foreach (UIWindow window in scene.Windows)
				{
					if (window.RootViewController is not UITabBarController tabs)
						continue;

					foreach (UITab tab in tabs.Tabs ?? [])
					{
						if (tab.Identifier == $"split:{Texts.Calendar}")
							return field = tab;
					}
				}
			}

			return null;
		}
	}

	
	void Apply()
	{
		lock (gate)
		{
			DateTime now = DateTime.Now;
			TimeOnly cutoff = config.Calendar.StartOfDay ?? TimeOnly.MinValue;

			DateOnly calculated = TimeOnly.FromDateTime(now) >= cutoff
				? DateOnly.FromDateTime(now)
				: DateOnly.FromDateTime(now).AddDays(-1);
			
			if (calculated != Value)
			{
				Value = calculated;
				logger.LogInformation("Current day changed to {Date}.", calculated);
			}

			DateTime next = now.Date.Add(cutoff.ToTimeSpan());
			if (next <= now)
				next = next.AddDays(1);
			
			TimeSpan delay = next - now;
			if (delay > MaxTimerDelay)
				delay = MaxTimerDelay;

			timer ??= new(_ => Refresh());
			timer.Change(delay, Timeout.InfiniteTimeSpan);

			if (OperatingSystem.IsIOSVersionAtLeast(26))
				ApplyTabIcon();
		}
	}
	
	void ApplyTabIcon()
	{
		if (appliedIconDay == Value ||
			CalendarTab is not UITab tab ||
			UIImage.GetSystemImage($"{Value.Day}.calendar") is not UIImage icon)
			return;

		tab.Image = icon;
		appliedIconDay = Value;
	}
	
	
	[ObservableProperty]
	public partial DateOnly Value { get; private set; }


	public void Refresh()
	{
		if (NSThread.IsMain)
		{
			Apply();
			return;
		}

		// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
		if (UIApplication.SharedApplication is not null)
			UIApplication.SharedApplication.BeginInvokeOnMainThread(Apply);
	}
}
