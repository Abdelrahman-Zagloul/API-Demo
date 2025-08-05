using System.ComponentModel.DataAnnotations;

namespace API_Demo.Dto
{
    public class RegisterUserDto
    {
        [Required, StringLength(30)]
        public string UserName { get; set; } = string.Empty;

        [Required, StringLength(200), EmailAddress]
        public string Email { get; set; } = string.Empty;


        [Required, Length(minimumLength: 6, maximumLength: 30, ErrorMessage = "Password Must Be Range 6 to 30 char")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "Confirm Password Must Match Password")]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
