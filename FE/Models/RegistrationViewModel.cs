using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class RegistrationViewModel
    {


        //[Required] public string Grade { get; set; }
        //[Required] public string Level { get; set; }
        //[Required] public string FullName { get; set; }
        //[Required, EmailAddress] public string Email { get; set; }
        //[Required, Phone] public string Phone { get; set; }
        //[Required] public int Gender { get; set; }
        //public int Status { get; set; }
        //public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);
        [Required] public string Grade { get; set; }
        [Required] public string Level { get; set; }
        [Required] public string FullName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, Phone] public string Phone { get; set; }
        [Required] public int Gender { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
