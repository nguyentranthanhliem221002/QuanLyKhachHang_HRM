using FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5000");
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        //// POST: /Account/Login
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    // Gửi dữ liệu lên BE API để đăng nhập
        //    var response = await _httpClient.PostAsJsonAsync("/api/account/login", model);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Lấy thông tin token hoặc user từ BE nếu có
        //        var result = await response.Content.ReadFromJsonAsync<dynamic>();

        //        // Lưu session, cookie hoặc TempData nếu muốn
        //        HttpContext.Session.SetString("UserName", model.UserName);

        //        // Redirect đến Home/Index
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Nếu login thất bại
        //    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
        //    return View(model);
        //}
        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if ((model.UserName == "admin" && model.Password == "123456") ||
                (model.UserName == "user1" && model.Password == "111111"))
            {
                HttpContext.Session.SetString("UserName", model.UserName);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(model);
        }

    }
}
