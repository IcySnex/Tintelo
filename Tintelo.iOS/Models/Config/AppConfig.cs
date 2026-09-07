using CommunityToolkit.Mvvm.ComponentModel;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public sealed partial class AppConfig(
	SimpleStorage storage) : ObservableObject
{
    [ObservableProperty]
    public partial bool BoolValue { get; set; } = storage.GetValue("configuration.boolValue", false);

    [ObservableProperty]
    public partial string StringValue { get; set; } = storage.GetValue("configuration.stringValue", string.Empty);

    [ObservableProperty]
    public partial int IntValue { get; set; } = storage.GetValue("configuration.intValue", 0);

    
	partial void OnBoolValueChanged(bool value) =>
		storage.SetValue("configuration.boolValue", value);

	partial void OnStringValueChanged(string value) =>
		storage.SetValue("configuration.stringValue", value);

	partial void OnIntValueChanged(int value) =>
		storage.SetValue("configuration.intValue", value);
}
