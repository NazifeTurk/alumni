namespace Alumni.Middlewares;

/// <summary>
/// Auth Middleware: Reads session data and attaches user info to HttpContext.Items
/// so all controllers can access the current user via context.Items["UserRole"] etc.
/// Does NOT block requests — use controller-level checks for protected routes.
/// </summary>
public class AuthSessionMiddleware
{
    private readonly RequestDelegate _next;

    public AuthSessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Pull session values and expose them as HttpContext.Items
        var userId   = context.Session.GetString("UserId");
        var userName = context.Session.GetString("UserName");
        var userRole = context.Session.GetString("UserRole");
        var userEmail = context.Session.GetString("UserEmail");

        if (!string.IsNullOrEmpty(userId))
        {
            context.Items["UserId"]    = userId;
            context.Items["UserName"]  = userName;
            context.Items["UserRole"]  = userRole;
            context.Items["UserEmail"] = userEmail;
            context.Items["IsLoggedIn"] = true;
        }
        else
        {
            context.Items["IsLoggedIn"] = false;
        }

        await _next(context);
    }
}

public static class AuthSessionMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthSession(this IApplicationBuilder builder)
        => builder.UseMiddleware<AuthSessionMiddleware>();
}
