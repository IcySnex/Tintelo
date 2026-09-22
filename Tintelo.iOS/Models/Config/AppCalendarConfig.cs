using CommunityToolkit.Mvvm.ComponentModel;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public partial class AppCalendarConfig : ObservableObject
{
	public AppCalendarConfig(
		SimpleStorage storage)
	{
		InitializeStoredProperties(storage);
	}
	
	
	[ObservableProperty]
	[StoreAs("configuration.calendar.firstdayofweek", FirstDayOfWeek.Automatic)]
	public partial FirstDayOfWeek FirstDayOfWeek { get; set; }
}