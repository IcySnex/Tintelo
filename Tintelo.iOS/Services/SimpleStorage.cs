using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Tintelo.iOS.Services;

public sealed class SimpleStorage(
	ILogger<SimpleStorage> logger)
{
	readonly NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

	
	public T GetValue<T>(
		string key,
		T defaultValue = default!)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(key);

		if (userDefaults[key] is not NSObject value)
		{
			logger.LogWarning("Failed to get value with key '{Key}' because it's not an NSObject.", key);
			return defaultValue;
		}

		object result = (value, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T)) switch
		{
			(NSString str, Type t) when t == typeof(string) => str.ToString(),
			(NSString str, Type t) when t == typeof(DateTime) => DateTime.ParseExact(str.ToString(), "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
			(NSString str, Type t) when t == typeof(DateTimeOffset) => DateTimeOffset.ParseExact(str.ToString(), "O", CultureInfo.InvariantCulture, DateTimeStyles.None),
			(NSString str, Type t) when t == typeof(DateOnly) => DateOnly.ParseExact(str.ToString(), "O", CultureInfo.InvariantCulture),
			(NSString str, Type t) when t == typeof(TimeOnly) => TimeOnly.ParseExact(str.ToString(), "O", CultureInfo.InvariantCulture),
			(NSString str, Type t) when t == typeof(TimeSpan) => TimeSpan.ParseExact(str.ToString(), "c", CultureInfo.InvariantCulture),
			(NSNumber num, Type t) when t.IsEnum => Enum.ToObject(t, num.Int64Value),
			(NSNumber num, Type t) when t == typeof(bool) => num.BoolValue,
			(NSNumber num, Type t) when t == typeof(int) => num.Int32Value,
			(NSNumber num, Type t) when t == typeof(long) => num.Int64Value,
			(NSNumber num, Type t) when t == typeof(float) => num.FloatValue,
			(NSNumber num, Type t) when t == typeof(double) => num.DoubleValue,

			_ => throw new InvalidOperationException($"The value stored for '{key}' cannot be read as {typeof(T).Name}.")
		};

		return (T)result;
	}
	
	public void SetValue<T>(
		string key,
		T value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(key);

		if (value is null)
		{
			logger.LogInformation("Value is null, removing '{key}'...", key);
			RemoveValue(key);
			return;
		}

		switch (value)
		{
			case string text:
				userDefaults.SetString(text, key);
				break;
			case bool boolean:
				userDefaults.SetBool(boolean, key);
				break;
			case int number:
				userDefaults.SetInt(number, key);
				break;
			case long number:
				userDefaults.SetInt((nint)number, key);
				break;
			case float number:
				userDefaults.SetFloat(number, key);
				break;
			case double number:
				userDefaults.SetDouble(number, key);
				break;
			case DateTime dateTime:
				userDefaults.SetString(dateTime.ToString("O", CultureInfo.InvariantCulture), key);
				break;
			case DateTimeOffset dateTimeOffset:
				userDefaults.SetString(dateTimeOffset.ToString("O", CultureInfo.InvariantCulture), key);
				break;
			case DateOnly date:
				userDefaults.SetString(date.ToString("O", CultureInfo.InvariantCulture), key);
				break;
			case TimeOnly time:
				userDefaults.SetString(time.ToString("O", CultureInfo.InvariantCulture), key);
				break;
			case TimeSpan duration:
				userDefaults.SetString(duration.ToString("c", CultureInfo.InvariantCulture), key);
				break;
			case Enum enumValue:
				userDefaults.SetInt((nint)Convert.ToInt64(enumValue), key);
				break;
			
			default:
				throw new NotSupportedException($"Values of type {typeof(T).Name} are not supported.");
		}
	}

	public void RemoveValue(
		string key)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(key);

		logger.LogInformation("Removing value with key '{Key}'...", key);
		
		userDefaults.RemoveObject(key);
	}
}
