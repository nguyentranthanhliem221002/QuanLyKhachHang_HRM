using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
    public class UpdateStudentRequest : UserBaseRequest
    {
        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        public DateTime EnrollmentDate { get; set; }  // 🔹 Thêm
        public int Status { get; set; }               // 🔹 Thêm
    }

}
