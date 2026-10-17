using SkeleKit;

namespace Tintelo.iOS.Utils;

public static class ColorsExtensions
{
	extension(Colors)
	{
		public static Color? ToolbarButton => OperatingSystem.IsIOSVersionAtLeast(26) ? Colors.Label : null;
	}
}