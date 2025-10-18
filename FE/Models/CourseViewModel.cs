namespace FE.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string TeacherName { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
