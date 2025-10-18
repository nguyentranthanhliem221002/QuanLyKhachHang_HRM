namespace BE.Models
{
    public class Notification : Base
    {
        public int Id { get; set; }
        public Guid ReceiverId { get; set; } // Student hoặc Employee
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }
    }
}
