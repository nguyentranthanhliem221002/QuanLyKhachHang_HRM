using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Dtos.Requests
{
    public class PaymentRequest
    {
        public Guid userId { get; set; }
        public Guid courseId { get; set; }
        public decimal amount { get; set; }
        public bool isPaid { get; set; } // ← thêm

    }
}
