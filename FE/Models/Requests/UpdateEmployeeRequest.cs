using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
   
    public class UpdateEmployeeRequest : UserBaseRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chức vụ")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cấp độ")]
        public string Level { get; set; }

        public decimal Salary { get; set; }
        public int Status { get; set; }
    }
}
