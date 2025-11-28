namespace FE.Models
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Fee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string SubjectName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        public bool IsPaid { get; set; } = false;

        public string? OrderId { get; set; }
        public string? TransactionId { get; set; }

    }
}
