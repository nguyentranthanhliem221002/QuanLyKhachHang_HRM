using FE.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FE.Services;

namespace FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService) => _accountService = accountService;

        [HttpGet]
        public IActionResult Login() => View();
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var message = await _accountService.RegisterAsync(model);

            if (string.IsNullOrEmpty(message))
            {
                ViewBag.Error = "Không thể đăng ký. Vui lòng thử lại.";
                return View(model);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(new UserViewModel
            {
                UserName = model.UserName,
                Password = model.Password
            });

            if (result == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.username ?? "UnknownUser"),
                new Claim(ClaimTypes.Email, result.email ?? ""),
                new Claim(ClaimTypes.Role, string.IsNullOrEmpty(result.role) ? "User" : result.role),
                new Claim(ClaimTypes.NameIdentifier, result.userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                }
            );

            return result.role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Teacher" => RedirectToAction("Dashboard", "Employee"),
                "Student" => RedirectToAction("Dashboard", "Student"),
                _ => RedirectToAction("Index", "Home")
            };

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // reset danh tính
            return RedirectToAction("Login", "Account");
        }
    }
}
