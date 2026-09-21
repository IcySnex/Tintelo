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
	string[] Options,
	Bindable<string?> SelectedOption,
	Color? Color = null) : SettingsEntry(Title, Icon)
{
	public readonly record struct PickerArguments(
		string[] Options,
		Bindable<string?> SelectedOption);

	public static PickerArguments Arguments<TSource, TOwner, TEnum>(
		TwoWayBindingExpression<TSource, TOwner, TEnum> selectedOption,
		Func<TEnum, string> titleFor) where TSource : class where TOwner : class? where TEnum : struct, Enum
	{
		TEnum[] values = System.Enum.GetValues<TEnum>();
		string[] options = values.Select(titleFor).ToArray();

		if (options.Any(string.IsNullOrEmpty))
			throw new ArgumentException("Every enum value needs a non-empty title.", nameof(titleFor));
		if (options.Distinct(StringComparer.Ordinal).Count() != options.Length)
			throw new ArgumentException("Every enum value needs a unique title.", nameof(titleFor));

		(BindingExpression<TSource, TOwner, TEnum> expression, Action<TOwner, TEnum> write) = selectedOption;
		Bindable<string?> binding = expression
			.ConvertTo(value =>
			{
				int index = Array.IndexOf(values, value);
				return options[index >= 0 ? index : 0];
			})
			.ConvertFrom(option =>
			{
				int index = Array.IndexOf(options, option);
				return values[index >= 0 ? index : 0];
			})
			.TwoWay(write);

		return new(options, binding);
	}


	public SettingsPickerEntry(
		string Title,
		string Icon,
		PickerArguments arguments,
		Color? color = null) : this(Title, Icon, arguments.Options, arguments.SelectedOption, color)
	{ }
}
