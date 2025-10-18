namespace FE.Models
{
    public class TestSubmitModel
    {
        public string Subject { get; set; } = null!;
        public Dictionary<int, string> Answers { get; set; } = new();
    }

}
