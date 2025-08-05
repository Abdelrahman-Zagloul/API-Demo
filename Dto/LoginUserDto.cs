using System.ComponentModel.DataAnnotations;

namespace API_Demo.Dto
{
    public class LoginUserDto
    {

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, Length(minimumLength: 6, maximumLength: 30, ErrorMessage = "Password Must Be Range 6 to 30 char")]
        public string Password { get; set; } = string.Empty;

    }
}
