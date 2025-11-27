using System.ComponentModel.DataAnnotations;

namespace FE.Models.Requests
{
    public class UpdateStudentRequest : UserBaseRequest
    {
        [Required] public string ClassName { get; set; }
        public string Grade { get; set; }
        public string Level { get; set; }
    }

}
