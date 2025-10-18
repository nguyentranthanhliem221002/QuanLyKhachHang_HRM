using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Employee : Person
    {
        public string Position { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Salary { get; set; }
    }
}

