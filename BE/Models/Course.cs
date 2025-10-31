using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Course : Base
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Fee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
     
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
