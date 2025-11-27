using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập Email hoặc Username")]
        public string UserName { get; set; } = string.Empty; // Có thể là email hoặc username

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
