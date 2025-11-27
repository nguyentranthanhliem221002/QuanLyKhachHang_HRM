using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
   
    public class UpdateEmployeeRequest : UserBaseRequest
    {
        [Required] public string Phone { get; set; }
        [Required] public string Position { get; set; }
        [Required] public string Level { get; set; }
        public decimal Salary { get; set; }
    }
}
