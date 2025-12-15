using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
    public class UserBaseRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

    }
}
