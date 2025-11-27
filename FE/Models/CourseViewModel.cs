namespace FE.Models
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Fee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string SubjectName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        // Trạng thái thanh toán dùng hiển thị ở FE
        public bool IsPaid { get; set; } = false;

        // Thêm OrderId để FE biết đơn thanh toán MoMo
        public string? OrderId { get; set; }
    }
}
