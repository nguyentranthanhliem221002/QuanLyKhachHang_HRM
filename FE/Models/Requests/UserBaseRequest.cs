//namespace FE.Models.Requests
//{
//    public class CreateUserRequest
//    {
//        public string? UserName { get; set; }
//        public string? Email { get; set; }
//        public string? FullName { get; set; }
//        public string? Password { get; set; }
//        public string? Role { get; set; }
//    }
//}
using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
    public class UserBaseRequest
    {
        [Required] public string FullName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
    }
}
