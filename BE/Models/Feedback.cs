using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Feedback : Base
    {
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } // 1–5 sao
        public DateTime FeedbackDate { get; set; }
    }
}
