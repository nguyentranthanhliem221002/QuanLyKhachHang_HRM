namespace BE.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;
        public string CorrectAnswer { get; set; } = null!; // A / B / C / D
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }
}
