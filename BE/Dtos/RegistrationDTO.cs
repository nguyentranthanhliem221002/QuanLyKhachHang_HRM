namespace BE.Dtos
{
    public class RegistrationDTO
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string Grade { get; set; }
        public string Level { get; set; }
        public int Status { get; set; } = 0;
    }

}
