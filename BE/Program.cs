using BE.Data;
using BE.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ====== 1️⃣ Cấu hình DbContext ======
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ====== 2️⃣ Cấu hình Identity ======
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ====== 3️⃣ Đọc URL FE từ appsettings.json ======
var feBaseUrl = builder.Configuration["Frontend:BaseUrl"] ?? "https://localhost:51746";

// ====== 4️⃣ Cấu hình CORS ======
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFE", policy =>
    {
        policy.WithOrigins(feBaseUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ====== 5️⃣ Cấu hình Controller + Swagger ======
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ====== 6️⃣ Migration + Seed Data ======
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    // 🧱 Tạo database nếu chưa có & chạy migration
    await db.Database.MigrateAsync();

    // 🌱 Gọi hàm Seed Data
    await DbInitializer.InitializeAsync(db, userManager, roleManager);
}

// ====== 7️⃣ Middleware ======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFE");

app.UseAuthentication();  // 🔑 Bắt buộc cho Identity
app.UseAuthorization();

app.MapControllers();

app.Run();
