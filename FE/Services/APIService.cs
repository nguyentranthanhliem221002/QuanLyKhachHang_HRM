using FE.Models;

namespace FE.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<List<CustomerViewModel>> GetCustomersAsync()
        //{
        //    var response = await _httpClient.GetAsync("api/customer");
        //    response.EnsureSuccessStatusCode();

        //    var data = await response.Content.ReadFromJsonAsync<List<CustomerViewModel>>();
        //    return data ?? new List<CustomerViewModel>();
        //}
    }

}
