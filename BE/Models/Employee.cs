using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Phone { get; set; } = string.Empty;   // thêm Phone
        public string Position { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,3)")]
        public decimal Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Level { get; set; } = string.Empty;
        public int Status { get; set; } = 1; // thêm Status (1 = Active, 0 = Inactive)

        // Liên kết 1-1 đến User
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}

