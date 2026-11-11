using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alumni.Middlewares;

/// <summary>
/// 🛡️ 404 Guardian Middleware
/// Intercepts all HTTP 404 Not Found responses across the pipeline and ensures
/// every 404 response returns a structured error with a clear "reason".
/// </summary>
public class NotFoundGuardianMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<NotFoundGuardianMiddleware> _logger;

    public NotFoundGuardianMiddleware(RequestDelegate next, ILogger<NotFoundGuardianMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var path = context.Request.Path.Value ?? "/";

                // 1. If this is the generic 404 page itself (/404), allow HTML to be returned with 404 status
                if (path.Equals("/404", StringComparison.OrdinalIgnoreCase))
                {
                    Alumni.Services.FileErrorLogger.LogError(404, context.Request.Method, path, "Generic 404 HTML page served", context.TraceIdentifier);
                    _logger.LogWarning("🛡️ [404 Guardian] Serving generic 404 HTML page for {Method} {Path}",
                        context.Request.Method, path);

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalBodyStream);
                    return;
                }

                // 2. If this is a non-API web request that resulted in 404, redirect (HTTP 302) to the generic 404 page
                if (!path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) &&
                    !path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
                {
                    Alumni.Services.FileErrorLogger.LogError(404, context.Request.Method, path, "Invalid web request redirected to generic 404 page", context.TraceIdentifier);
                    _logger.LogWarning("🛡️ [404 Guardian] Redirecting invalid web request {Method} {Path} to /404",
                        context.Request.Method, path);

                    context.Response.Body = originalBodyStream;
                    context.Response.Redirect($"/404?requestedPath={Uri.EscapeDataString(path)}");
                    return;
                }

                // 3. For API routes, generate structured JSON with reason
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(memoryStream).ReadToEndAsync();

                string reason = DetermineReason(context, responseText);

                // Build standard structured 404 Guardian response
                var guardianPayload = new JsonObject
                {
                    ["status"] = "error",
                    ["code"] = 404,
                    ["reason"] = reason,
                    ["path"] = context.Request.Path.Value ?? "/",
                    ["method"] = context.Request.Method,
                    ["timestamp"] = DateTime.UtcNow.ToString("o")
                };

                // Preserve original message and hint if provided
                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    try
                    {
                        var jsonNode = JsonNode.Parse(responseText);
                        if (jsonNode is JsonObject obj)
                        {
                            if (obj["message"] != null)
                            {
                                guardianPayload["message"] = obj["message"]!.GetValue<string>();
                            }
                            if (obj["hint"] != null)
                            {
                                guardianPayload["hint"] = obj["hint"]!.GetValue<string>();
                            }
                        }
                    }
                    catch
                    {
                        // Response was plain text or non-JSON; keep structured payload
                    }
                }

                if (guardianPayload["hint"] == null)
                {
                    guardianPayload["hint"] = "Check endpoint syntax, verify resource IDs, or explore available endpoints at /api/swagger.";
                }

                var finalJson = JsonSerializer.Serialize(guardianPayload, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.Body = originalBodyStream;
                await context.Response.WriteAsync(finalJson);

                // 📝 Log 404 error to logs/error.logs
                Alumni.Services.FileErrorLogger.LogError(404, context.Request.Method, context.Request.Path.Value ?? "/", reason, context.TraceIdentifier);

                _logger.LogWarning("🛡️ [404 Guardian] {Method} {Path} -> Reason: {Reason}",
                    context.Request.Method, context.Request.Path, reason);
            }
            else
            {
                // Not a 404, copy original response stream to the client
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private static string DetermineReason(HttpContext context, string? existingBody)
    {
        var path = context.Request.Path.Value ?? "/";
        var method = context.Request.Method;

        // 1. If an existing message or reason was provided by a controller, use it
        if (!string.IsNullOrWhiteSpace(existingBody))
        {
            try
            {
                var parsed = JsonNode.Parse(existingBody);
                if (parsed is JsonObject obj)
                {
                    if (obj["reason"] != null) return obj["reason"]!.ToString();
                    if (obj["message"] != null) return obj["message"]!.ToString();
                }
            }
            catch { }
        }

        // 2. Route-based reason determination
        if (path.StartsWith("/api/users/") && path.EndsWith("/jobs"))
        {
            return $"User specified in path '{path}' does not exist. Cannot create nested job posting.";
        }

        if (path.StartsWith("/api/jobs/") && path.EndsWith("/applications"))
        {
            return $"Job posting or candidate user specified in '{path}' does not exist.";
        }

        if (path.StartsWith("/api/users/"))
        {
            return $"User entity with the requested ID does not exist in the database.";
        }

        if (path.StartsWith("/api/jobs/"))
        {
            return $"Job posting entity with the requested ID does not exist in the database.";
        }

        if (path.StartsWith("/api/announcements/"))
        {
            return $"Announcement entity with the requested ID does not exist in the database.";
        }

        if (path.StartsWith("/api/"))
        {
            return $"API route '{path}' does not match any registered endpoint for HTTP method '{method}'.";
        }

        return $"The requested route '{path}' was not found on this server.";
    }
}

public static class NotFoundGuardianMiddlewareExtensions
{
    /// <summary>
    /// Registers the 404 Guardian Middleware into the HTTP pipeline.
    /// </summary>
    public static IApplicationBuilder UseNotFoundGuardian(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NotFoundGuardianMiddleware>();
    }

    /// <summary>
    /// Alias: UseGuardian404
    /// </summary>
    public static IApplicationBuilder UseGuardian404(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NotFoundGuardianMiddleware>();
    }
}
