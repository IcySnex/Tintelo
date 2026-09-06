using Microsoft.Extensions.Logging;

namespace Tintelo.iOS.Logging.Providers;

public sealed class ConsoleLoggerProvider : ILoggerProvider
{
	sealed class ConsoleLogger(
		ConsoleLoggerProvider provider,
		string category) : ILogger
	{
		public IDisposable? BeginScope<TState>(
			TState state) where TState : notnull =>
			null;

		public bool IsEnabled(
			LogLevel logLevel) =>
			logLevel != LogLevel.None;

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception? exception,
			Func<TState, Exception?, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			string message = formatter(state, exception);
			if (string.IsNullOrEmpty(message) && exception is null)
				return;

			provider.Write(logLevel, category, message, exception);
		}
	}


	readonly Lock @lock = new();


	void Write(
		LogLevel level,
		string category,
		string message,
		Exception? exception)
	{
		lock (@lock)
			LogEntryFormatter.Write(Console.Out, null, level, category, message, exception);
	}


	public ILogger CreateLogger(
		string categoryName) =>
		new ConsoleLogger(this, categoryName);

	public void Dispose()
	{ }
}
