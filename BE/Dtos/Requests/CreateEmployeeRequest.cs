//using System.ComponentModel.DataAnnotations;

//namespace FE.Models.Requests
//{
//    public class CreateEmployeeRequest : CreateUserRequest
//    {
//        [Required]
//        public string Phone { get; set; }

//        [Required]
//        public string Position { get; set; }

//        [Required]
//        public string Level { get; set; } // Junior / Middle / Senior

//        public decimal Salary { get; set; }

//        public int Status { get; set; } = 1; // 1 = Active, 0 = Inactive
//    }
//}
using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{

    public class CreateEmployeeRequest : UserBaseRequest
    {
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int Status { get; set; }   // 🔹 Thêm

    }
}
