using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string RoleType { get; set; } = string.Empty;
        public int Status { get; set; } // 0 = Pending, 1 = Active
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }


        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? EnrollmentDate { get; set; }


    }
}
