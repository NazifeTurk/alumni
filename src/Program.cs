using Alumni.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (Controllers, Views, Services)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseAuthorization();

// 🚪 Week 02: Register Route Modules
app.MapBasicRoutes();
app.MapAlumniApiRoutes();

// 🚫 Slide Section D & E: ANY /* -> 404 Not Found Fallback Handler
app.MapFallback((HttpContext context) =>
{
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    return Results.Json(new
    {
        status = "error",
        code = 404,
        message = $"Route '{context.Request.Path}' does not exist on this server.",
        method = context.Request.Method,
        hint = "Refer to / (or /home) for the routing table, or /api/alumni for resources."
    });
});

app.Run();
