using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Payment : Base
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Amount { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Course Course { get; set; }

    }
}
