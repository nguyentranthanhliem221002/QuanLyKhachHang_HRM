namespace FE.Models
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<CourseViewModel> MyCourses { get; set; } = new();
        public List<FeedbackViewModel> Feedbacks { get; set; } = new();
    }
}
