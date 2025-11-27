using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class RegistrationViewModel
    {
        [Required]
        public Guid UserId { get; set; }  // Dùng gửi kèm học viên đang đăng ký

        [Required]
        public Guid CourseId { get; set; }  // Khóa học chọn để đăng ký

        [Required]
        public string Grade { get; set; } = string.Empty;

        [Required]
        public string Level { get; set; } = string.Empty;
        public int Status { get; set; } = 0;

    
    }
}
