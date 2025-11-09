using FE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;

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


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


var backendUrl = builder.Environment.EnvironmentName == "Docker"
    ? Environment.GetEnvironmentVariable("API_URL_SERVER")    // FE container gọi BE container
    : Environment.GetEnvironmentVariable("API_URL_CLIENT")      // FE ngoài container, trình duyệt gọi IP public
      ?? builder.Configuration["BackendApi:BaseUrl"]
      ?? "https://localhost:51745";

Console.WriteLine($"👉 Backend API Base URL: {backendUrl}");



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
builder.Services.AddHttpClient<PaymentService>(client =>
{
    client.BaseAddress = new Uri(backendUrl);
});



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
