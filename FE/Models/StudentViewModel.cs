namespace FE.Models
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        // Danh sách khóa học đã đăng ký, kèm trạng thái thanh toán
        public List<CourseViewModel> MyCourses { get; set; } = new List<CourseViewModel>();
    }
}

