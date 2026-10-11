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

// Root endpoint for Week 1 Project Inception
app.MapGet("/", () => Results.Ok(new
{
    project = "Alumni Tracking System",
    status = "online",
    runtime = ".NET 7 / ASP.NET Core",
    current_week = "01 - Project inception & fundamentals",
    message = "Welcome to the Alumni Tracking System API."
}));

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    service = "alumni-dotnet-app"
}));

app.MapControllers();

app.Run();
