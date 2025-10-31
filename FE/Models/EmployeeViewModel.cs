namespace FE.Models
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }             
        public string FullName { get; set; }    
        public string Email { get; set; }        
        public string Phone { get; set; }       
        public string Position { get; set; }    
        public decimal Salary { get; set; }    
        public int Status { get; set; }          
        public DateTime DateOfJoining { get; set; } 
        public string Level { get; set; }
    }
}
