using FE.Models;
using System.Net.Http.Json;

namespace FE.Services
{
    public class CourseService
    {
        private readonly HttpClient _http;

        public CourseService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesAsync()
        {
            var result = await _http.GetFromJsonAsync<List<CourseViewModel>>("api/course");
            return result ?? new List<CourseViewModel>();
        }

        public async Task<CourseViewModel?> GetCourseByIdAsync(int id)
        {
            var result = await _http.GetFromJsonAsync<CourseViewModel>($"api/course/{id}");
            return result;
        }

        public async Task<string> CreateCourseAsync(CourseViewModel model)
        {
            var resp = await _http.PostAsJsonAsync("api/course", model);
            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());

            return "Tạo khóa học thành công";
        }

        public async Task<string> UpdateCourseAsync(int id, CourseViewModel model)
        {
            var resp = await _http.PutAsJsonAsync($"api/course/{id}", model);
            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());

            return "Cập nhật khóa học thành công";
        }

        public async Task DeleteCourseAsync(int id)
        {
            var resp = await _http.DeleteAsync($"api/course/{id}");
            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());
        }
    }
}
