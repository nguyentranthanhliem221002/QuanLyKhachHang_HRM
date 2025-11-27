using FE.Models;
using FE.Models.Requests;
using System.Net.Http.Json;

namespace FE.Services
{
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        // ===========================
        // ✅ Lấy danh sách toàn bộ user
        // ===========================
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var result = await _http.GetFromJsonAsync<List<UserViewModel>>("api/users");
            return result ?? new List<UserViewModel>();
        }

        // ===========================
        // ✅ Lấy danh sách nhân viên
        // ===========================
        public async Task<List<EmployeeViewModel>> GetAllEmployeesAsync()
        {
            var result = await _http.GetFromJsonAsync<List<EmployeeViewModel>>("api/users/employees");
            return result ?? new List<EmployeeViewModel>();
        }

        // ===========================
        // ✅ Lấy danh sách học viên
        // ===========================
        public async Task<List<UserViewModel>> GetAllStudentsAsync()
        {
            var result = await _http.GetFromJsonAsync<List<UserViewModel>>("api/users/students");
            return result ?? new List<UserViewModel>();
        }

        // ===========================
        // ✅ Thêm học viên
        // ===========================
        public async Task<string> AddStudentAsync(CreateStudentRequest req)
        {
            var resp = await _http.PostAsJsonAsync("api/users/students", req);
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi tạo học viên: {error}");
            }

            var message = await resp.Content.ReadFromJsonAsync<dynamic>();
            return message?.message ?? "Tạo học viên thành công";
        }
        public async Task<UserViewModel?> GetStudentByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<UserViewModel>($"api/users/students/{id}");
        }

        //===========================
        //✅ Sửa học viên
        //===========================
        public async Task<string> UpdateStudentAsync(Guid id, CreateStudentRequest req)
        {
            var resp = await _http.PutAsJsonAsync($"api/users/students/{id}", req);
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi cập nhật học viên: {error}");
            }

            var message = await resp.Content.ReadFromJsonAsync<dynamic>();
            return message?.message ?? "Cập nhật học viên thành công";
        }

        public async Task<string> UpdateStudentAsync(Guid id, UpdateStudentRequest req)
        {
            var resp = await _http.PutAsJsonAsync($"api/users/students/{id}", req);
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi cập nhật học viên: {error}");
            }

            var message = await resp.Content.ReadFromJsonAsync<dynamic>();
            return message?.message ?? "Cập nhật học viên thành công";
        }
        // ===========================
        // ✅ Xóa học viên
        // ===========================
        public async Task DeleteStudentAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/users/students/{id}");
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi xóa học viên: {error}");
            }
        }

        // ===========================
        // ✅ Thêm nhân viên
        // ===========================
        public async Task<string> AddEmployeeAsync(CreateEmployeeRequest req)
        {
            var resp = await _http.PostAsJsonAsync("api/users/employees", req);
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi tạo nhân viên: {error}");
            }

            var message = await resp.Content.ReadFromJsonAsync<dynamic>();
            return message?.message ?? "Tạo nhân viên thành công";
        }
        // ===========================
        // ✅ Lấy thông tin học viên theo ID
        // ===========================
        public async Task<UserViewModel?> GetEmployeeByIdAsync(Guid id)
        {
            var result = await _http.GetFromJsonAsync<UserViewModel>($"api/users/employees/{id}");
            return result;
        }

        // ===========================
        // ✅ Sửa nhân viên
        // ===========================
        //public async Task<string> UpdateEmployeeAsync(Guid id, CreateEmployeeRequest req)
        //{
        //    var resp = await _http.PutAsJsonAsync($"api/users/employees/{id}", req);
        //    if (!resp.IsSuccessStatusCode)
        //    {
        //        var error = await resp.Content.ReadAsStringAsync();
        //        throw new Exception($"Lỗi khi cập nhật nhân viên: {error}");
        //    }

        //    var message = await resp.Content.ReadFromJsonAsync<dynamic>();
        //    return message?.message ?? "Cập nhật nhân viên thành công";
        //}
    
        public async Task<string> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest req)
        {
            var resp = await _http.PutAsJsonAsync($"api/users/employees/{id}", req);
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi cập nhật nhân viên: {error}");
            }

            var message = await resp.Content.ReadFromJsonAsync<dynamic>();
            return message?.message ?? "Cập nhật nhân viên thành công";
        }
        // ===========================
        // ✅ Xóa nhân viên
        // ===========================
        public async Task DeleteEmployeeAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"api/users/employees/{id}");
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi khi xóa nhân viên: {error}");
            }
        }
    }
}
