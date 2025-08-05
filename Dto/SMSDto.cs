using System.ComponentModel.DataAnnotations;

namespace API_Demo.Dto
{
    public class SendSMSDto
    {
        [Required(ErrorMessage = "The Phone Number is Required.")]
        [RegularExpression(@"^\+201[0125][0-9]{8}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }


        [Required]
        [MaxLength(200)]
        public string Body { get; set; }
    }
}
