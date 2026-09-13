using CommunityToolkit.Mvvm.Input;
using SkeleKit;

namespace Tintelo.iOS.Models.Settings;

public abstract record SettingsEntry(
	string Title,
	string Icon);

public sealed record SettingsActionEntry(
	string Title,
	string Icon,
	Color? Color,
	IAsyncRelayCommand Command) : SettingsEntry(Title, Icon);

public sealed record SettingsDisplayEntry(
	string Title,
	string Icon,
	string Value) : SettingsEntry(Title, Icon);

public sealed record SettingsNavigationEntry(
	string Title,
	string Icon,
	Type ViewModelType) : SettingsEntry(Title, Icon);

public sealed record SettingsToggleEntry(
	string Title,
	string Icon,
	Bindable<bool> IsOn) : SettingsEntry(Title, Icon);

public sealed record SettingsPickerEntry(
	string Title,
	string Icon,
	string[] Options,
	Bindable<string?> SelectedOption) : SettingsEntry(Title, Icon);
