using BE.Data;
using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===========================================
// 1️⃣ Kestrel config cho Docker
// ===========================================
if (builder.Environment.EnvironmentName == "Docker")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080); // HTTP nội bộ
        options.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps("/https/aspnetapp.pfx", "123456");
        });
    });
}


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


var feUrls = builder.Configuration.GetSection("Frontend:Urls").Get<string[]>()
             ?? new[] { "https://localhost:51746", "https://localhost:51745", "https://localhost:5001", "https://localhost:5000", "https://98.95.20.86:5001", "https://98.95.20.86:5000" };

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    if (!(await db.Database.CanConnectAsync()))
    {
        await db.Database.MigrateAsync();
    }

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
app.Run();
