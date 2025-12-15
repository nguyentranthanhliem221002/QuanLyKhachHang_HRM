using System.ComponentModel.DataAnnotations;

namespace BE.Dtos.Requests
{
   
    public class UpdateEmployeeRequest : UserBaseRequest
    {
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int Status { get; set; }   
    }
}
