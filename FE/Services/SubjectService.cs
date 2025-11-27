using FE.Models;

namespace FE.Services
{
    public class SubjectService
    {
        private readonly HttpClient _httpClient;

        public SubjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Lấy tất cả môn học
        public async Task<List<SubjectViewModel>> GetAllSubjectsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<SubjectViewModel>>("api/subject");
            return result ?? new List<SubjectViewModel>();
        }
    }
}
