using System.ComponentModel.DataAnnotations.Schema;

namespace FE.Models
{
    public class PaymentRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }

        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } // ← thêm

        public string? OrderId { get; set; }

    }
}
