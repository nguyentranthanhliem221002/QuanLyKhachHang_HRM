using FE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// =========================================================
// 2️⃣ Cấu hình Authentication (Cookie)
// =========================================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// =========================================================
// 3️⃣ Cấu hình Session
// =========================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================================================
// 4️⃣ Load cấu hình từ appsettings + environment
// =========================================================
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


//var backendUrl = Environment.GetEnvironmentVariable("API_URL")
//                 ?? builder.Configuration["BackendApi:BaseUrl"]
//                 ?? "https://be:443";
var backendUrl = Environment.GetEnvironmentVariable("API_URL_SERVER")
                 ?? builder.Configuration["BackendApi:BaseUrl"]
                 ?? "https://98.95.20.86:443";


builder.Services.AddHttpClient<UserService>(c => c.BaseAddress = new Uri(backendUrl))
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

Console.WriteLine($"👉 Backend API Base URL: {backendUrl}");

Action<HttpClient> configureClient = client => client.BaseAddress = new Uri(backendUrl);
Func<HttpMessageHandler> configureHandler = () => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

builder.Services.AddHttpClient<UserService>(configureClient)
    .ConfigurePrimaryHttpMessageHandler(configureHandler);

builder.Services.AddHttpClient<RegistrationService>(configureClient)
    .ConfigurePrimaryHttpMessageHandler(configureHandler);

builder.Services.AddHttpClient<AccountService>(configureClient)
    .ConfigurePrimaryHttpMessageHandler(configureHandler);

builder.Services.AddHttpClient<AdminService>(configureClient)
    .ConfigurePrimaryHttpMessageHandler(configureHandler);

builder.Services.AddHttpClient<TestService>(configureClient)
    .ConfigurePrimaryHttpMessageHandler(configureHandler);

builder.Services.AddHttpClient<CourseService>(configureClient)
    .ConfigurePrimaryHttpMessageHandler(configureHandler);


var app = builder.Build();



// Hiển thị lỗi chi tiết trong Development hoặc Docker
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Bật HTTPS & Static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing
app.UseRouting();

// Authentication + Session phải nằm TRƯỚC Authorization
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
