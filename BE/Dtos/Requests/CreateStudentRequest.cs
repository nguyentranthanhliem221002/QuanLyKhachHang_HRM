using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
    public class CreateStudentRequest : UserBaseRequest
    {

        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        public string ClassName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public int Status { get; set; } = 0;
    }

}
