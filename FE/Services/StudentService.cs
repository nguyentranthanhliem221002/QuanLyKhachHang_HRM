using FE.Models;
using System.Net.Http.Json;

namespace FE.Services
{
    public class StudentService
    {
        private readonly HttpClient _http;

        public StudentService(HttpClient http) => _http = http;

        public async Task<List<UserViewModel>> GetAllStudentsAsync()
        {
            return await _http.GetFromJsonAsync<List<UserViewModel>>("api/user/students");
        }

        public async Task<bool> RegisterAsync(UserViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", model);
            return response.IsSuccessStatusCode;
        }

    }
}
