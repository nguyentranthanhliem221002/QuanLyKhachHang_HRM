using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models
{
    public class Student 
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;   
        public string Level { get; set; } = string.Empty;  
        public DateTime EnrollmentDate { get; set; }
        public string Phone { get; set; } = string.Empty;

        public int Status { get; set; }

        // Liên kết 1-1 đến User
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
