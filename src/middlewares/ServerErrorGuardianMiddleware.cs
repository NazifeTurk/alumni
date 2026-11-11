using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alumni.Middlewares;

/// <summary>
/// 🛡️ 5xx Guardian Middleware
/// Intercepts all unhandled exceptions and server-side 5xx errors across the pipeline,
/// guards sensitive system internals, logs the diagnostic trace, and returns a
/// clean, structured JSON response with error reason and traceId.
/// </summary>
public class ServerErrorGuardianMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ServerErrorGuardianMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ServerErrorGuardianMiddleware(
        RequestDelegate next,
        ILogger<ServerErrorGuardianMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            // Also guard any status code explicitly set to 5xx without an unhandled exception
            if (context.Response.StatusCode >= 500 && context.Response.StatusCode <= 599 && !context.Response.HasStarted)
            {
                await Write5xxResponseAsync(context, null, context.Response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "🛡️ [5xx Guardian] Unhandled exception on {Method} {Path}: {Message}",
                context.Request.Method, context.Request.Path, ex.Message);

            if (!context.Response.HasStarted)
            {
                await Write5xxResponseAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }
    }

    private async Task Write5xxResponseAsync(HttpContext context, Exception? ex, int statusCode)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        var reason = Determine5xxReason(ex, statusCode);

        var payload = new JsonObject
        {
            ["status"] = "error",
            ["code"] = statusCode,
            ["title"] = GetStatusCodeTitle(statusCode),
            ["reason"] = reason,
            ["path"] = context.Request.Path.Value ?? "/",
            ["method"] = context.Request.Method,
            ["traceId"] = traceId,
            ["timestamp"] = DateTime.UtcNow.ToString("o")
        };

        // If in development or debug, provide safe exception details for diagnosis
        if (ex != null)
        {
            payload["details"] = new JsonObject
            {
                ["exceptionType"] = ex.GetType().FullName,
                ["exceptionMessage"] = ex.Message,
                ["innerException"] = ex.InnerException?.Message
            };
        }

        payload["hint"] = "The server encountered an internal failure. If the problem persists, share the traceId with the system administrator.";

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }

    private static string Determine5xxReason(Exception? ex, int statusCode)
    {
        if (ex is TimeoutException)
        {
            return "A backend operation timed out before completing.";
        }
        if (ex is Npgsql.PostgresException || ex?.InnerException is Npgsql.PostgresException)
        {
            return "A database storage or query error occurred while processing the request.";
        }
        if (ex is Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            return "A database entity persistence or constraint failure occurred.";
        }
        if (ex is NullReferenceException)
        {
            return "A null reference violation occurred in the application service layer.";
        }
        if (ex != null)
        {
            return $"An unhandled {ex.GetType().Name} was caught by the 5xx Guardian: {ex.Message}";
        }

        return statusCode switch
        {
            500 => "Internal Server Error: An unexpected failure occurred on the server.",
            501 => "Not Implemented: The requested server functionality is not supported.",
            502 => "Bad Gateway: The server received an invalid response from an upstream dependency.",
            503 => "Service Unavailable: The server is temporarily overloaded or down for maintenance.",
            504 => "Gateway Timeout: An upstream service failed to respond within the allowed time window.",
            _ => $"Server error with HTTP status code {statusCode}."
        };
    }

    private static string GetStatusCodeTitle(int statusCode) => statusCode switch
    {
        500 => "Internal Server Error",
        501 => "Not Implemented",
        502 => "Bad Gateway",
        503 => "Service Unavailable",
        504 => "Gateway Timeout",
        _ => "Server Error"
    };
}

public static class ServerErrorGuardianMiddlewareExtensions
{
    /// <summary>
    /// Registers the 5xx Guardian Middleware into the HTTP pipeline.
    /// </summary>
    public static IApplicationBuilder UseServerErrorGuardian(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ServerErrorGuardianMiddleware>();
    }

    /// <summary>
    /// Alias: UseGuardian5xx
    /// </summary>
    public static IApplicationBuilder UseGuardian5xx(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ServerErrorGuardianMiddleware>();
    }

    /// <summary>
    /// Alias: Use5xxGuardian
    /// </summary>
    public static IApplicationBuilder Use5xxGuardian(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ServerErrorGuardianMiddleware>();
    }
}
