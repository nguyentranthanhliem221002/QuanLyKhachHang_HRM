using BE.Data;
using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kestrel config cho Docker/Prod
if (builder.Environment.IsProduction() || builder.Environment.EnvironmentName == "Docker")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080);
        options.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps("/https/aspnetapp.pfx", "123456");
        });
    });
}

// Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// CORS FE URLs (dev + prod)
var feUrls = builder.Configuration.GetSection("Frontend:Urls").Get<string[]>()
             ?? new[] {
                 "https://localhost:51746",
                 "http://localhost:51746",
                 "https://13.223.107.213:5001",
                 "http://13.223.107.213:8080"
             };
Console.WriteLine($"🔥 CORS Allowed Origins: {string.Join(", ", feUrls)}");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFE", policy =>
    {
        policy.WithOrigins(feUrls)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Database init
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    if (!(await db.Database.CanConnectAsync()))
        await db.Database.MigrateAsync();

    await DbInitializer.InitializeAsync(db, userManager, roleManager);
}

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker" || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFE");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine($"🚀 BE is running in {app.Environment.EnvironmentName} mode");
app.Run();
