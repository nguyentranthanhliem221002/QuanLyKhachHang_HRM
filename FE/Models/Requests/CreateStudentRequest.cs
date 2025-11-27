using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
    public class CreateStudentRequest : UserBaseRequest
    {

        [Required] public string ClassName { get; set; }
        [Required] public string Grade { get; set; }
        [Required] public string Level { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }

   
}
