using Alumni.Models;
using Alumni.Services;

namespace Alumni.Routes;

public static class UserApiRoutes
{
    public static void MapUserApiRoutes(this WebApplication app)
    {
        // 1. GET /api/health -> Rich JSON health check
        app.MapGet("/api/health", () => Results.Ok(new
        {
            status = "ok",
            service = "Alumni Tracking System API",
            environment = app.Environment.EnvironmentName,
            timestamp = DateTime.UtcNow,
            currentWeek = "Week 03 — HTTP Methods & CRUD",
            storage = "In-Memory (Thread-Safe ConcurrentDictionary)",
            database = "Not connected yet (As instructed: 'don't use any database yet')",
            uptime = TimeSpan.FromMilliseconds(Environment.TickCount64).ToString(@"dd\.hh\:mm\:ss")
        }))
        .WithName("HealthCheck")
        .WithTags("Health");

        // Map /api/alumni as alias for REST resources
        MapCrudEndpoints(app, "/api/alumni");
    }

    private static void MapCrudEndpoints(WebApplication app, string prefix)
    {
        var group = app.MapGroup(prefix)
            .WithTags(prefix.Contains("users") ? "Users" : "Alumni");

        // 4. GET /api/users -> List all users (200 OK)
        group.MapGet("/", (InMemoryUserService userService) =>
        {
            var users = userService.GetAll();
            return Results.Ok(users);
        })
        .WithName($"GetAll_{prefix.Trim('/')}")
        .Produces<IEnumerable<User>>(StatusCodes.Status200OK);

        // 4b. GET /api/users/{id} -> Get single user by ID (200 OK or 404 Not Found)
        group.MapGet("/{id:int}", (int id, InMemoryUserService userService) =>
        {
            var user = userService.GetById(id);
            if (user == null)
            {
                return Results.NotFound(new
                {
                    status = "error",
                    code = 404,
                    message = $"Resource with ID '{id}' was not found in in-memory store.",
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(user);
        })
        .WithName($"GetById_{prefix.Trim('/')}")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // 3. POST /api/users -> Create user in-memory (201 Created)
        group.MapPost("/", (CreateUserRequest request, InMemoryUserService userService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest(new
                {
                    status = "error",
                    code = 400,
                    message = "The 'name' field is required."
                });
            }

            var createdUser = userService.Create(request);

            return Results.Created($"{prefix}/{createdUser.Id}", createdUser);
        })
        .WithName($"Create_{prefix.Trim('/')}")
        .Produces<User>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // 5a. PUT /api/users/{id} -> Replace whole user (200 OK or 404 Not Found)
        group.MapPut("/{id:int}", (int id, UpdateUserRequest request, InMemoryUserService userService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest(new
                {
                    status = "error",
                    code = 400,
                    message = "All fields (name, email, graduationYear, role) must be provided for PUT (replace whole)."
                });
            }

            var updatedUser = userService.Update(id, request);
            if (updatedUser == null)
            {
                return Results.NotFound(new
                {
                    status = "error",
                    code = 404,
                    message = $"Cannot update. Resource with ID '{id}' does not exist.",
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(updatedUser);
        })
        .WithName($"Update_{prefix.Trim('/')}")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        // 5b. PATCH /api/users/{id} -> Change one/partial field (200 OK or 404 Not Found)
        group.MapPatch("/{id:int}", (int id, PatchUserRequest request, InMemoryUserService userService) =>
        {
            var patchedUser = userService.Patch(id, request);
            if (patchedUser == null)
            {
                return Results.NotFound(new
                {
                    status = "error",
                    code = 404,
                    message = $"Cannot patch. Resource with ID '{id}' does not exist.",
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(patchedUser);
        })
        .WithName($"Patch_{prefix.Trim('/')}")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // 6. DELETE /api/users/{id} -> Remove user from in-memory store (200 OK or 404 Not Found)
        group.MapDelete("/{id:int}", (int id, InMemoryUserService userService) =>
        {
            var removed = userService.Delete(id);
            if (!removed)
            {
                return Results.NotFound(new
                {
                    status = "error",
                    code = 404,
                    message = $"Cannot delete. Resource with ID '{id}' does not exist.",
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(new
            {
                status = "success",
                code = 200,
                message = $"Resource with ID '{id}' was successfully removed from in-memory store.",
                timestamp = DateTime.UtcNow
            });
        })
        .WithName($"Delete_{prefix.Trim('/')}")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
