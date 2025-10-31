using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = null!;


        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string RoleType { get; set; } = "Student";

        public string? ClassName { get; set; } = "DefaultClass"; // ✅ trùng BE

        public DateTime EnrollmentDate { get; set; } = DateTime.Now; 

        public int Status { get; set; } = 0; 

        public bool IsActive { get; set; } = true;

        public string? Phone { get; set; }
        public string? Position { get; set; }
        public string? Level { get; set; }
        public decimal Salary { get; set; }
        public DateTime? DateOfJoining { get; set; }

    }
}
