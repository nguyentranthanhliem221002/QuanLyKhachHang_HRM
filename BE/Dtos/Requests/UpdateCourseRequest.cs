namespace BE.Dtos.Requests
{
    public class UpdateCourseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SubjectId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
    }
}
