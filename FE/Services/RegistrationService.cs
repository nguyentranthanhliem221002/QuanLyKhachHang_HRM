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

        public async Task<bool> RegisterStudentAsync(RegistrationViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/course/register", model);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Lỗi đăng ký: {response.StatusCode} - {error}");
                return false;
            }

            return true;
        }

        public async Task<string> PayWithMoMoAsync(Guid userId, Guid courseId, decimal amount)
        {
            var model = new { UserId = userId, CourseId = courseId, Amount = amount };
            var resp = await _http.PostAsJsonAsync("api/payment/momo", model);
            var result = await resp.Content.ReadFromJsonAsync<dynamic>();
            return result.payUrl;
        }
    }
}
