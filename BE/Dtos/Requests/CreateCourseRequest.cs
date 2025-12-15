namespace BE.Dtos.Requests
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // decimal(18,3) mapping ở entity
        public decimal Fee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid SubjectId { get; set; }

        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
    }
}
