using System.ComponentModel.DataAnnotations;

namespace BE.Dtos
{
    public class StudentDTO
    {
        [Required]
        public string Grade { get; set; } = string.Empty;

        [Required]
        public string Level { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public int Gender { get; set; } // 0=Nam, 1=Nữ, 2=Khác

        public int Status { get; set; } = 0;
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

        // Thêm ClassName để khớp Model
        public string ClassName { get; set; } = string.Empty;

        // Thêm các thông tin liên quan nếu cần (Enrollments, UserId...)
        public Guid UserId { get; set; }
    }
}
