using FE.Models;
using System.Net.Http.Json;
namespace FE.Services
{
    public class RegistrationService
    {
        private readonly HttpClient _http;

        public RegistrationService(HttpClient http)
        {
            _http = http;
        }

        // Gửi đăng ký học viên + khóa học
        public async Task<bool> RegisterStudentAsync(RegistrationViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/student/register", model);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Lỗi đăng ký: {response.StatusCode} - {error}");
                return false;
            }

            return true;
        }
    }
}
