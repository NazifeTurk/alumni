namespace Alumni.Services;

/// <summary>
/// Thread-safe file logger for application errors.
/// Appends all 4xx client errors, 5xx server errors, and unhandled exceptions to logs/error.logs.
/// </summary>
public static class FileErrorLogger
{
    private static readonly object _lock = new();

    public static void LogError(
        int statusCode,
        string method,
        string path,
        string reason,
        string? traceId = null,
        Exception? exception = null)
    {
        try
        {
            var directories = new List<string>
            {
                Path.Combine(Directory.GetCurrentDirectory(), "logs"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"),
                "/app/logs"
            };

            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var trace = !string.IsNullOrEmpty(traceId) ? $" [TraceId: {traceId}]" : string.Empty;
            var logEntry = $"[{timestamp}] [{statusCode}] {method} {path} - Reason: {reason}{trace}";

            if (exception != null)
            {
                logEntry += $"{Environment.NewLine}  Exception: {exception.GetType().FullName}: {exception.Message}";
                if (!string.IsNullOrEmpty(exception.StackTrace))
                {
                    logEntry += $"{Environment.NewLine}  StackTrace: {exception.StackTrace.Trim()}";
                }
            }

            logEntry += Environment.NewLine;

            lock (_lock)
            {
                var written = false;
                foreach (var dir in directories.Distinct())
                {
                    try
                    {
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        // Write to logs/error.logs (as explicitly specified: logs/error.logs)
                        var primaryLogPath = Path.Combine(dir, "error.logs");
                        File.AppendAllText(primaryLogPath, logEntry);

                        // Also write to logs/error.log for standard log viewer compatibility
                        var altLogPath = Path.Combine(dir, "error.log");
                        File.AppendAllText(altLogPath, logEntry);

                        written = true;
                    }
                    catch
                    {
                        // Directory not writable or accessible, continue to next candidate
                    }
                }

                if (!written)
                {
                    // Fallback to local logs directory
                    var fallbackDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                    Directory.CreateDirectory(fallbackDir);
                    File.AppendAllText(Path.Combine(fallbackDir, "error.logs"), logEntry);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FileErrorLogger Notice] Could not append to logs/error.logs: {ex.Message}");
        }
    }
}
