using FE.Models;
using System.Net.Http.Json;

namespace FE.Services
{
    public class AccountService
    {
        private readonly HttpClient _http;

        public AccountService(HttpClient http)
        {
            _http = http;
        }

        // ======================================================
        // 🔹 Đăng nhập
        // ======================================================
        public async Task<LoginResult?> LoginAsync(UserViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new
            {
                UserName = model.UserName,   // ✅ giống hệt property trong LoginRequest
                Password = model.Password     // ✅ giống hệt property trong LoginRequest
            });


            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResult>();
        }

        // ======================================================
        // 🔹 Đăng ký tài khoản
        // ======================================================
        public async Task<string?> RegisterAsync(UserViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", new
            {
                userName = model.UserName,
                email = model.Email,
                fullName = model.FullName,
                password = model.Password,
                confirmPassword = model.ConfirmPassword,
                role = model.RoleType,          // ✅ trùng key Swagger
                className = model.ClassName,
                enrollmentDate = model.EnrollmentDate,
                status = model.Status
            });

            if (!response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var result = await response.Content.ReadFromJsonAsync<RegisterResult>();
            return result?.message;
        }

        // ======================================================
        // 🔹 Models nội bộ
        // ======================================================
        public class LoginResult
        {
            public string username { get; set; } = null!;
            public string fullname { get; set; } = null!;
            public string role { get; set; } = null!;
            public string message { get; set; } = null!;
        }

        private class RegisterResult
        {
            public string message { get; set; } = null!;
        }
    }
}
