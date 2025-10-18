namespace BE.Models
{
    public class Department : BaseItem
    {
        public int Id { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
