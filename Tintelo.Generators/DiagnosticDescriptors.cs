using Microsoft.CodeAnalysis;

namespace Tintelo.Generators;

internal static class DiagnosticDescriptors
{
	public static readonly DiagnosticDescriptor MissingObservableProperty = new(
		id: "TINTEL001",
		title: "Stored property must be observable",
		messageFormat: "Property '{0}' must also have [ObservableProperty] when using [StoreAs]",
		category: "Tintelo.Generators",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor InvalidContainingType = new(
		id: "TINTEL002",
		title: "Stored property must be declared in a supported type",
		messageFormat: "Property '{0}' must be declared in a top-level, non-generic partial class",
		category: "Tintelo.Generators",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor DuplicateStorageKey = new(
		id: "TINTEL003",
		title: "Storage key is used more than once",
		messageFormat: "Storage key '{0}' is also used by property '{1}'",
		category: "Tintelo.Generators",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor InvalidStorageKey = new(
		id: "TINTEL004",
		title: "Storage key is invalid",
		messageFormat: "Property '{0}' must use a non-empty storage key",
		category: "Tintelo.Generators",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor InvalidDefaultValue = new(
		id: "TINTEL005",
		title: "Default value is incompatible with the property",
		messageFormat: "The default value for property '{0}' cannot be converted to '{1}'",
		category: "Tintelo.Generators",
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);
}
