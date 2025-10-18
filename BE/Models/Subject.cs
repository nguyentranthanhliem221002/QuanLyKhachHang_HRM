namespace BE.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // Toán, Lý, Hóa, Sinh, Ngữ văn, Kỹ năng mềm
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
