//using BE.Data;
//using BE.Models;

//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// ===========================================
//// ✅ Load cấu hình từ appsettings + environment
//// ===========================================
//builder.Configuration
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables();  // Đọc các biến từ docker-compose/.env


//// ====== 1️⃣ Cấu hình DbContext ======
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//// ====== 2️⃣ Cấu hình Identity ======
//builder.Services.AddIdentity<User, IdentityRole<Guid>>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

//// ====== 3️⃣ Đọc URL FE từ appsettings.json ======
//var feBaseUrl = builder.Configuration["Frontend:BaseUrl"] ?? "https://localhost:51746";

//// ====== 4️⃣ Cấu hình CORS ======
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFE", policy =>
//    {
//        policy.WithOrigins(feBaseUrl)
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

//// ====== 5️⃣ Cấu hình Controller + Swagger ======
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// ====== 6️⃣ Migration + Seed Data ======
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var db = services.GetRequiredService<ApplicationDbContext>();
//    var userManager = services.GetRequiredService<UserManager<User>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

//    // 🧱 Tạo database nếu chưa có & chạy migration
//    await db.Database.MigrateAsync();

//    // 🌱 Gọi hàm Seed Data
//    await DbInitializer.InitializeAsync(db, userManager, roleManager);
//}

//// ====== 7️⃣ Middleware ======
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseCors("AllowFE");

//app.UseAuthentication();  // 🔑 Bắt buộc cho Identity
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using BE.Data;
using BE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===========================================
// ✅ Load cấu hình từ appsettings + environment
// ===========================================
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();  // Đọc biến môi trường từ Docker / .env

// ====== 1️⃣ Cấu hình DbContext ======
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====== 2️⃣ Cấu hình Identity ======
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ====== 3️⃣ Cấu hình CORS linh hoạt ======
var feUrls = builder.Configuration.GetSection("Frontend:Urls").Get<string[]>()
             ?? new[] { "https://localhost:51746", "http://localhost:5001", "http://fe:8080" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFE", policy =>
    {
        policy.WithOrigins(feUrls)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ====== 4️⃣ Controller + Swagger ======
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====== 5️⃣ Build app ======
var app = builder.Build();

// ====== 6️⃣ Migration + Seed Data ======
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    //await db.Database.MigrateAsync();
    if (!(await db.Database.CanConnectAsync()))
    {
        await db.Database.MigrateAsync();
    }

    await DbInitializer.InitializeAsync(db, userManager, roleManager);
}

// ====== 7️⃣ Middleware ======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection(); // chỉ bật https ở local
}

app.UseCors("AllowFE");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ====== 8️⃣ Kestrel config cho Docker ======
if (app.Environment.EnvironmentName == "Docker")
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080); // Docker chỉ chạy HTTP
    });
}

app.Run();
