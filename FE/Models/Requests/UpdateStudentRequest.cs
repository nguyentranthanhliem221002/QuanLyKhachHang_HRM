using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
    public class UpdateStudentRequest : UserBaseRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        public string ClassName { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public int Status { get; set; }
    }

}
