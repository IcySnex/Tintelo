using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Tintelo.iOS.Services;

public partial class SystemInfo(
	ILogger<SystemInfo> logger)
{
	// ReSharper disable once InconsistentNaming
	[LibraryImport("libc", StringMarshalling = StringMarshalling.Utf8)]
	private static partial int sysctlbyname(
		[MarshalAs(UnmanagedType.LPStr)] string name,
		[Out] byte[] oldp,
		ref int oldlen,
		IntPtr newp,
		uint newlen);


	public string GetDeviceModel()
	{
		logger.LogInformation("Getting device model...");
		
		byte[] modelIdentifierBytes = new byte[256];
		int size = 256;

		string identifier = sysctlbyname("hw.machine", modelIdentifierBytes, ref size, IntPtr.Zero, 0) == 0 ? Encoding.ASCII.GetString(modelIdentifierBytes, 0, size).TrimEnd('\0') : "N/A";
		return $"{UIDevice.CurrentDevice.Model} ({identifier})";
	}

	public string GetOperatingSystem()
	{
		logger.LogInformation("Getting operating system...");

		return $"{UIDevice.CurrentDevice.SystemName} ({UIDevice.CurrentDevice.SystemVersion})";
	}

	public int GetBatteryLevel()
	{
		logger.LogInformation("Getting batter level...");

		UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
		float level = UIDevice.CurrentDevice.BatteryLevel;
		UIDevice.CurrentDevice.BatteryMonitoringEnabled = false;
		
		return (int)(level * 100);
	}
}