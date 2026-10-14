using Alumni.Data;
using Alumni.Routes;
using Alumni.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// 🗄️ Week 05: PostgreSQL Database & Entity Framework Core ORM
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=db;Port=5432;Database=alumni_db;Username=alumni_user;Password=alumni_password";

builder.Services.AddDbContext<AlumniDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Register In-Memory Services (Week 03 & Week 04 Fallback Stores)
builder.Services.AddSingleton<InMemoryUserService>();
builder.Services.AddSingleton<IAnnouncementService, InMemoryAnnouncementService>();

// 📖 Week 03 Task 7: Swagger / OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Alumni Tracking System API",
        Version = "v1",
        Description = "Alumni Tracking System — MVC, PostgreSQL ORM & Persistent CRUD (Assoc. Prof. Dr. Emre Akadal)",
        Contact = new OpenApiContact
        {
            Name = "Nazife Türk",
            Email = "nazife.turk@ogr.iu.edu.tr"
        }
    });
});

var app = builder.Build();

// Automatically ensure PostgreSQL tables exist when database is ready
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AlumniDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("[Database] PostgreSQL connection established and tables verified.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Database Notice] PostgreSQL not reachable at startup ({ex.Message}). Will connect when containers are up.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 📖 Expose Swagger at both /api/swagger and /swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alumni API v1");
    c.RoutePrefix = "api/swagger";
});

// Also support root /swagger redirect/view
app.MapGet("/swagger", () => Results.Redirect("/api/swagger"));

app.UseRouting();
app.UseAuthorization();

// 🚪 Week 02: Basic & Whiteboard Routes
app.MapBasicRoutes();

// 🔄 Week 03: Minimal User API Routes
app.MapUserApiRoutes();

// 🏛️ Week 04: MVC Controllers (UserController, ApiUserController, AnnouncementController, ApiAnnouncementController)
app.MapControllers();

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
        hint = "Inspect available endpoints at /api/swagger or visit /home."
    });
});

app.Run();
