using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
    public class CreateStudentRequest : UserBaseRequest
    {

        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        // Thêm các property cần thiết cho controller
        public DateTime EnrollmentDate { get; set; }  // ngày ghi danh
        public int Status { get; set; }               // trạng thái học viên
    }

}
