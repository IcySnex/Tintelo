using CommunityToolkit.Mvvm.Input;
using SkeleKit;

namespace Tintelo.iOS.Models.Settings;

public abstract record SettingsEntry(
	string Title,
	string Icon);

public sealed record SettingsActionEntry(
	string Title,
	string Icon,
	IAsyncRelayCommand Command,
	Color? Color = null) : SettingsEntry(Title, Icon);

public sealed record SettingsDisplayEntry(
	string Title,
	string Icon,
	Bindable<string?> Text) : SettingsEntry(Title, Icon);

public sealed record SettingsNavigationEntry(
	string Title,
	string Icon,
	Type ViewModelType,
	Bindable<string?>? Text = null) : SettingsEntry(Title, Icon);

public sealed record SettingsToggleEntry(
	string Title,
	string Icon,
	Bindable<bool> IsOn) : SettingsEntry(Title, Icon);

public sealed record SettingsPickerEntry(
	string Title,
	string Icon,
	IReadOnlyList<string> Options,
	Bindable<string?> SelectedOption,
	Color? Color = null) : SettingsEntry(Title, Icon);