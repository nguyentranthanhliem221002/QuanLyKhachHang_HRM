using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
    public class CreateStudentRequest : UserBaseRequest
    {
       
        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        public string ClassName { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public int Status { get; set; } = 1;
    }

   
}
