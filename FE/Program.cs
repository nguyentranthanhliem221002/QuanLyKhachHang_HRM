//using FE.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();
//builder.Services.AddHttpContextAccessor();

//// Authentication
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login";
//        options.AccessDeniedPath = "/Account/AccessDenied";
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//    });

//// Session
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//// Load configuration
//builder.Configuration
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables();

//// Determine Backend URL
//string backendUrl;

//if (builder.Environment.IsProduction())
//{
//    // Production: Dùng IP public của BE server
//    backendUrl = Environment.GetEnvironmentVariable("API_URL_CLIENT")
//                 ?? builder.Configuration["BackendApi:BaseUrl"]
//                 ?? "https://98.95.20.86:5000";
//}
//else if (builder.Environment.EnvironmentName == "Docker")
//{
//    // Docker local: Dùng container name
//    backendUrl = Environment.GetEnvironmentVariable("API_URL_SERVER")
//                 ?? "https://be:443";
//}
//else
//{
//    // Development: Dùng localhost
//    backendUrl = builder.Configuration["BackendApi:BaseUrl"]
//                 ?? "https://localhost:51745";
//}

//Console.WriteLine($"👉 Environment: {builder.Environment.EnvironmentName}");
//Console.WriteLine($"👉 Backend API Base URL: {backendUrl}");

//// Configure HttpClient
//Action<HttpClient> configureClient = client => client.BaseAddress = new Uri(backendUrl);
//Func<HttpMessageHandler> configureHandler = () => new HttpClientHandler
//{
//    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
//};

//builder.Services.AddHttpClient<UserService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
//builder.Services.AddHttpClient<RegistrationService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
//builder.Services.AddHttpClient<AccountService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
//builder.Services.AddHttpClient<AdminService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
//builder.Services.AddHttpClient<TestService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
//builder.Services.AddHttpClient<CourseService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
//builder.Services.AddHttpClient<PaymentService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);

//var app = builder.Build();

//// Middleware
//if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication();
//app.UseSession();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//Console.WriteLine($"🚀 FE is running in {app.Environment.EnvironmentName} mode");
//app.Run();

using FE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Backend API URL tùy môi trường
string backendUrl = builder.Environment.IsProduction()
    ? builder.Configuration["BackendApi:BaseUrl"] ?? "https://98.95.20.86:5000"
    : builder.Environment.EnvironmentName == "Docker"
        ? Environment.GetEnvironmentVariable("API_URL_SERVER") ?? "https://be:443"
        : builder.Configuration["BackendApi:BaseUrl"] ?? "https://localhost:5000";

Console.WriteLine($"👉 FE Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"👉 Backend API Base URL: {backendUrl}");

// HttpClient config
Action<HttpClient> configureClient = client => client.BaseAddress = new Uri(backendUrl);
Func<HttpMessageHandler> configureHandler = () => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

builder.Services.AddHttpClient<UserService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
builder.Services.AddHttpClient<RegistrationService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
builder.Services.AddHttpClient<AccountService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
builder.Services.AddHttpClient<AdminService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
builder.Services.AddHttpClient<TestService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
builder.Services.AddHttpClient<CourseService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);
builder.Services.AddHttpClient<PaymentService>(configureClient).ConfigurePrimaryHttpMessageHandler(configureHandler);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Console.WriteLine($"🚀 FE is running in {app.Environment.EnvironmentName} mode");
app.Run();
