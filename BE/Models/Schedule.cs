namespace BE.Models
{
    public class Schedule : Base
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Room { get; set; }
        public string Instructor { get; set; } // Hoặc liên kết đến Employee
    }
}
