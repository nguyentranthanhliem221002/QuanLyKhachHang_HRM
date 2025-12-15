using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FE.Services
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;

        // Constructor nhận trực tiếp HttpClient (typed client)
        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(int Students, int Employees, int Courses)> GetDashboardStatsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<DashboardStats>("api/admin/dashboard");
            if (response == null) return (0, 0, 0);
            return (response.Students, response.Employees, response.Courses);
        }

        // Class private để map dữ liệu từ API
        private class DashboardStats
        {
            public int Students { get; set; }
            public int Employees { get; set; }
            public int Courses { get; set; }
        }
    }
}
