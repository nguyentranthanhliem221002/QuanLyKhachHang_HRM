namespace BE.Models
{
    public class Person : Base
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Gender { get; set; }

    }
}
