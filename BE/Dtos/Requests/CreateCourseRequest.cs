namespace BE.Dtos.Requests
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // decimal(18,3) mapping ở entity
        public decimal Fee { get; set; }

        // Không nullable: khóa học phải có ngày bắt đầu/kết thúc
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Giữ chỉ Id để liên kết Subject
        public Guid SubjectId { get; set; }

        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
    }
}
