using System.Diagnostics;

namespace Alumni.Middlewares;

/// <summary>
/// First Custom Middleware: Logging Middleware
/// Measures request execution duration and logs a single line to the terminal:
/// Format: time method route restype duration
/// Example: 10:20:30 POST /user 201 18ms
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestTime = DateTime.Now.ToString("HH:mm:ss");

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            var method = context.Request.Method;
            var route = context.Request.Path.Value ?? "/";
            var restype = context.Response.StatusCode;
            var duration = $"{stopwatch.ElapsedMilliseconds}ms";

            // Single line terminal output: time method route restype duration
            // Example: 10:20:30 POST /user 201 18ms
            var logLine = $"{requestTime} {method} {route} {restype} {duration}";

            // Write directly to stdout terminal
            Console.WriteLine(logLine);

            // Also send to ASP.NET Core ILogger infrastructure
            _logger.LogInformation("{Time} {Method} {Route} {StatusCode} {Duration}",
                requestTime, method, route, restype, duration);

            // 📝 If response is 4xx client error (400, 409, 415), append to logs/error.logs
            if (restype >= 400 && restype != 404 && restype < 500)
            {
                Alumni.Services.FileErrorLogger.LogError(restype, method, route, $"HTTP {restype} Client Error ({duration})", context.TraceIdentifier);
            }
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
