using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string RoleType { get; set; } = string.Empty; // "Student" hoặc "Teacher"
        public int Status { get; set; }                      // 0 = Pending, 1 = Active
        public bool IsActive { get; set; }

        // Đăng ký / cập nhật mật khẩu
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        // Thông tin học viên
        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? EnrollmentDate { get; set; }

    }
}
