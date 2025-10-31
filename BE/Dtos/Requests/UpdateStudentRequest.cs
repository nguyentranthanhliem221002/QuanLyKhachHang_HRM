using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
    public class UpdateStudentRequest : UserBaseRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        public string ClassName { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public int Status { get; set; }
    }

}
