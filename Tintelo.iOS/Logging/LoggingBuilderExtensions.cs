using Microsoft.Extensions.Logging;
using Tintelo.iOS.Logging.Providers;

namespace Tintelo.iOS.Logging;

public static class LoggingBuilderExtensions
{
	extension(ILoggingBuilder builder)
	{
		public ILoggingBuilder AddConsole()
		{
			builder.AddProvider(new ConsoleLoggerProvider());
			return builder;
		}

		public ILoggingBuilder AddFile(
			string directoryPath,
			string fileNamePrefix = "Log-",
			int retainedFileCount = 10)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(directoryPath);
			ArgumentOutOfRangeException.ThrowIfLessThan(retainedFileCount, 1);

			builder.AddProvider(new FileLoggerProvider(directoryPath, fileNamePrefix, retainedFileCount));
			return builder;
		}
	}
}
