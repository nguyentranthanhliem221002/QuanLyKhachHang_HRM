using FE.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public AccountController(IHttpClientFactory factory) => _factory = factory;

        [HttpGet]
        public IActionResult Login() => View();
        public IActionResult AccessDenied() => View();
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Xoá danh tính người dùng hiện tại ngay lập tức
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Chuyển hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _factory.CreateClient("BE");
            var response = await client.PostAsJsonAsync("api/auth/login", new
            {
                UserName = model.UserName,
                Password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            if (result == null)
            {
                ViewBag.Error = "Không nhận được phản hồi hợp lệ từ máy chủ!";
                return View(model);
            }

            // ✅ Tạo Claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, result.username),
        new Claim(ClaimTypes.Role, result.role)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // ✅ Cấu hình cookie (tùy chọn)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Giữ cookie nếu user không logout
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            // ✅ Đăng nhập và tạo cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // ✅ Điều hướng theo role
            return result.role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Teacher" => RedirectToAction("Dashboard", "Teacher"),
                "Student" => RedirectToAction("Dashboard", "Student"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        //[HttpGet]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Login");
        //}

        private class LoginResult
        {
            public string username { get; set; } = null!;
            public string role { get; set; } = null!;
        }
    }
}
