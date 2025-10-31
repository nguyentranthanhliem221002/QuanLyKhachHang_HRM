using FE.Models;
using System.Net.Http.Json;

namespace FE.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _http;
        public EmployeeService(HttpClient http) => _http = http;

        // Lấy tất cả user có role là "Employee"
        public async Task<List<UserViewModel>> GetAllEmployeesAsync() =>
            await _http.GetFromJsonAsync<List<UserViewModel>>("api/user/employees");

        public async Task<UserViewModel> AddEmployeeAsync(UserViewModel emp)
        {
            var resp = await _http.PostAsJsonAsync("api/user", emp);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<UserViewModel>();
        }

        public async Task<UserViewModel> UpdateEmployeeAsync(Guid id, UserViewModel emp)
        {
            var resp = await _http.PutAsJsonAsync($"api/user/{id}", emp);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<UserViewModel>();
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/user/{id}");
            resp.EnsureSuccessStatusCode();
        }
    }
}
