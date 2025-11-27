namespace BE.Dtos
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int Status { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Level { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }

}
