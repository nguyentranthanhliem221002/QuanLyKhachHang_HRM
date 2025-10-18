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
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; } // "Cash", "BankTransfer", "Online"
        public int Status { get; set; } 
    }
}
