using FE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 🔹 1️⃣ Cấu hình MVC + các dịch vụ cơ bản
// =========================================================
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// =========================================================
// 🔹 2️⃣ Cấu hình Authentication (Cookie)
// =========================================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";         // Trang login mặc định
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang bị từ chối quyền
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Hết hạn sau 30p
    });

// =========================================================
// 🔹 3️⃣ Cấu hình Session
// =========================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================================================
// 🔹 4️⃣ Cấu hình URL Backend API
// =========================================================
var backendUrl = Environment.GetEnvironmentVariable("API_URL")
                 ?? builder.Configuration["BackendApi:BaseUrl"];

Console.WriteLine($"👉 Backend API Base URL: {backendUrl}"); // debug log

// =========================================================
// 🔹 5️⃣ Đăng ký HttpClient cho các Service
// =========================================================
builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddHttpClient<RegistrationService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddHttpClient<AccountService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddHttpClient<AdminService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddHttpClient<TestService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});
builder.Services.AddHttpClient<CourseService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});
// =========================================================
// 🔹 6️⃣ Build app
// =========================================================
var app = builder.Build();

// =========================================================
// 🔹 7️⃣ Middleware pipeline (theo đúng thứ tự chuẩn ASP.NET Core)
// =========================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ⚠️ Authentication và Session phải nằm TRƯỚC Authorization
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

// =========================================================
// 🔹 8️⃣ Định tuyến mặc định
// =========================================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// =========================================================
// 🔹 9️⃣ Chạy ứng dụng
// =========================================================
app.Run();
