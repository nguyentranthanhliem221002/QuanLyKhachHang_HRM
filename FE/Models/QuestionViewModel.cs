namespace FE.Models
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;
    }
}
