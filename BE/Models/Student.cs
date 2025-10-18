namespace BE.Models
{
    public class Student : Person
    {
        public string StudentCode { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
