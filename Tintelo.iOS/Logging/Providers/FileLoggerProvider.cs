using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Tintelo.iOS.Logging.Providers;

public sealed class FileLoggerProvider(
	string directoryPath,
	string fileNamePrefix = "Log-",
	int retainedFileCount = 10) : ILoggerProvider
{
	sealed class FileLogger(
		FileLoggerProvider provider,
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
	
	StreamWriter? writer;
	DateOnly? writerDate;
	bool disposed;

	
	
	void EnsureWriter(
		DateOnly date)
	{
		if (writer is not null && writerDate == date)
			return;

		writer?.Dispose();
		Directory.CreateDirectory(directoryPath);

		string filePath = Path.Combine(
			directoryPath,
			$"{fileNamePrefix}{date:yyyy-MM-dd}.txt");

		writer = new(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read), new UTF8Encoding(false))
		{
			AutoFlush = true
		};
		writerDate = date;

		DeleteExpiredFiles();
	}

	void DeleteExpiredFiles()
	{
		string searchPattern = $"{fileNamePrefix}*.txt";
		IEnumerable<string> expiredFiles = Directory
			.EnumerateFiles(directoryPath, searchPattern)
			.OrderByDescending(Path.GetFileName, StringComparer.Ordinal)
			.Skip(retainedFileCount);

		foreach (string file in expiredFiles)
			File.Delete(file);
	}

	
	internal void Write(
		LogLevel level,
		string category,
		string message,
		Exception? exception)
	{
		lock (@lock)
		{
			if (disposed)
				return;

			try
			{
				DateTimeOffset now = DateTimeOffset.Now;
				EnsureWriter(DateOnly.FromDateTime(now.DateTime));

				LogEntryFormatter.Write(writer!, now, level, category, message, exception);
			}
			catch (Exception writeException)
			{
				Debug.WriteLine($"Could not write to the application log: {writeException}");
			}
		}
	}

	
	public ILogger CreateLogger(
		string categoryName) =>
		new FileLogger(this, categoryName);
	
	public void Dispose()
	{
		lock (@lock)
		{
			if (disposed)
				return;

			disposed = true;
			writer?.Dispose();
			writer = null;
		}
	}
}
