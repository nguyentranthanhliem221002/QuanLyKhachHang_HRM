using FE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

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

string backendUrl = builder.Environment.IsProduction()
    ? builder.Configuration["BackendApi:BaseUrl"] ?? "https://98.95.20.86:5000"
    : builder.Environment.EnvironmentName == "Docker"
        ? Environment.GetEnvironmentVariable("API_URL_SERVER") ?? "https://be:443"
        : builder.Configuration["BackendApi:BaseUrl"] ?? "https://localhost:5000";

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

app.Run();
