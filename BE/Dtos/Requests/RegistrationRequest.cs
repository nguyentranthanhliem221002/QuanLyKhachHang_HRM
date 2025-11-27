namespace BE.Dtos.Requests
{
    public class RegistrationRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public string Grade { get; set; }
        public string Level { get; set; }
        public int Status { get; set; } = 0;
    }
}
