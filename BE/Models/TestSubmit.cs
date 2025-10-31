namespace BE.Models
{
    public class TestSubmit
    {
        public string Subject { get; set; } = null!;
        public Dictionary<int, string> Answers { get; set; } = new();
    }
}
