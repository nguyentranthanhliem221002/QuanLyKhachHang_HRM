namespace BE.Dtos
{
    public class CourseDTO
    {
        public Guid Id { get; set; }           // Trước là int → đổi thành Guid
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Grade { get; set; }
        public string Level { get; set; }

        public Guid SubjectId { get; set; }    // Trước là int → đổi thành Guid
        public string SubjectName { get; set; }
        public bool IsPaid { get; set; }

    }

}
