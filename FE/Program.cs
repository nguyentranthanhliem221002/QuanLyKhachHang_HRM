using FE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Thêm MVC
builder.Services.AddControllersWithViews();

// 👉 Thêm dòng này để dùng IHttpContextAccessor trong view
builder.Services.AddHttpContextAccessor();

// ✅ Thêm Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// ✅ Session (vẫn giữ nếu bạn cần lưu tạm biến phụ)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpClient("BE", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                  ?? Environment.GetEnvironmentVariable("API_URL")
                  ?? "https://localhost:51745/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<APIService>();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Bắt buộc: thứ tự đúng Authentication → Session → Authorization
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
