using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; } = true; // Mặc định active

        public ICollection<Payment> Payments { get; set; }
    }

}
