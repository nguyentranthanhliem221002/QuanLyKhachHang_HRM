namespace FE.Models
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; } 
        public string Content { get; set; } 
        public int Rating { get; set; } // 1–5 sao
        public DateTime Date { get; set; }
    }
}
