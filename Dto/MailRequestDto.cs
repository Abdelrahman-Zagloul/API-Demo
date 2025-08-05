using System.ComponentModel.DataAnnotations;

namespace API_Demo.Dto
{
    public class MailRequestDto
    {
        [Required,EmailAddress]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
