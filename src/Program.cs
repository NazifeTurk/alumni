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

// 🛠️ Week 05 & 06: Register Database-backed CRUD Services
builder.Services.AddScoped<IUserService, DbUserService>();
builder.Services.AddScoped<IAnnouncementService, DbAnnouncementService>();
builder.Services.AddScoped<IJobPostingService, DbJobPostingService>();

// Fallback In-Memory stores for development / testing without containers
builder.Services.AddSingleton<InMemoryUserService>();
builder.Services.AddSingleton<InMemoryAnnouncementService>();

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

// 🗄️ Item ⑤ & ⑥: Automatically verify PostgreSQL schema and execute Factory Seeders
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AlumniDbContext>();
    try
    {
        db.Database.Migrate();
        DatabaseSeeder.Seed(db);
        Console.WriteLine("[Database] PostgreSQL connection established, migrations applied, and seed completed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Database Notice] Migration notice ({ex.Message}). Trying EnsureCreated...");
        try
        {
            db.Database.EnsureCreated();
            DatabaseSeeder.Seed(db);
        }
        catch (Exception innerEx)
        {
            Console.WriteLine($"[Database Notice] PostgreSQL not reachable at startup ({innerEx.Message}). Will connect when containers are up.");
        }
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
