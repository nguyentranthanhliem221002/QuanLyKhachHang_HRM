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

        public async Task<CourseViewModel?> GetCourseByIdAsync(Guid id)
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

        public async Task<string> UpdateCourseAsync(Guid id, CourseViewModel model)
        {
            var resp = await _http.PutAsJsonAsync($"api/course/{id}", model);
            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());

            return "Cập nhật khóa học thành công";
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/course/{id}");
            if (!resp.IsSuccessStatusCode)
                throw new Exception(await resp.Content.ReadAsStringAsync());
        }

        public async Task<List<CourseViewModel>> GetCoursesByStudentIdAsync(Guid studentId)
        {
            var result = await _http.GetFromJsonAsync<List<CourseViewModel>>($"api/course/student/{studentId}");
            return result ?? new List<CourseViewModel>();
        }
        // Gửi đăng ký học viên + khóa học
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

    }
}
