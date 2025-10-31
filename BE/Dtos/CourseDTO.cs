namespace BE.Dtos
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Fee { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
    }

}
