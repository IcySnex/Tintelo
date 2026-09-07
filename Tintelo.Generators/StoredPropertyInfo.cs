using Microsoft.CodeAnalysis;

namespace Tintelo.Generators;

internal sealed class StoredPropertyInfo(
	IPropertySymbol property,
	string? key,
	bool hasDefaultValue,
	TypedConstant defaultValue,
	Location location)
{
	public IPropertySymbol Property { get; } = property;

	public string? Key { get; } = key;

	public bool HasDefaultValue { get; } = hasDefaultValue;

	public TypedConstant DefaultValue { get; } = defaultValue;

	public Location Location { get; } = location;
}