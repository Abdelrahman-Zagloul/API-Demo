using System.ComponentModel.DataAnnotations;

namespace API_Demo.Dto
{
    public class AddRoleDto
    {
        [Required, StringLength(30)]
        public string Role { get; set; } = string.Empty;

        [Required, StringLength(36)]
        public string UserId { get; set; }=string.Empty;
    }
}
