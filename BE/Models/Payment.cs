using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Payment : Base
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string Method { get; set; } = "MoMo"; // hoặc "Cash", "BankTransfer"
        public int Status { get; set; } // 0=pending, 1=success, 2=failed

        public string? TransactionId { get; set; }
        public string? Description { get; set; }
    }
}
