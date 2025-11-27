namespace FE.Models
{
    public class PaymentViewModel
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
