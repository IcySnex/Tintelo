using CommunityToolkit.Mvvm.ComponentModel;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public sealed partial class AppConfig : ObservableObject
{
	public AppConfig(SimpleStorage storage) =>
		InitializeStoredProperties(storage);

	[ObservableProperty]
	[StoreAs("configuration.boolValue", false)]
	public partial bool BoolValue { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.stringValue", "")]
	public partial string StringValue { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.intValue", 0)]
	public partial int IntValue { get; set; }
}
