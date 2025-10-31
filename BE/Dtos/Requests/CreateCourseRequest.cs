namespace BE.Dtos.Requests
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Fee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SubjectId { get; set; }   // Chỉ cần Id, không cần Subject object
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
    }
}
