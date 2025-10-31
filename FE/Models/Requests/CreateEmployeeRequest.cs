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

namespace FE.Models.Requests
{

    public class CreateEmployeeRequest : UserBaseRequest
    {
     
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chức vụ")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cấp độ")]
        public string Level { get; set; }

        public decimal Salary { get; set; } = 0;
        public int Status { get; set; }
    }
}
