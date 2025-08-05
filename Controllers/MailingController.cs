using Microsoft.AspNetCore.Mvc;
using API_Demo.Dto;
using API_Demo.Services;

namespace API_Demo.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class NotificationController : ControllerBase
    {

        private readonly IMailService _mailService;
        private readonly ISMSService _sMSService;
        public NotificationController(IMailService mailService, ISMSService sMSService)
        {
            _mailService = mailService;
            _sMSService = sMSService;
        }

        [HttpPost("Send Email (MailKit)")]
        public async Task<IActionResult> SendEmailAsync([FromForm] MailRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mailService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
            return Ok();

        }
        [HttpPost("Send Email (.Net)")]
        public async Task<IActionResult> SendEmailUsingDotNet([FromForm] MailRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _mailService.SendEmailUsingDotNet(dto.ToEmail, dto.Subject, dto.Body,dto.Attachments);
            return Ok();

        }
        [HttpPost("Send SMS")]
        public async Task<IActionResult> SendSMS(SendSMSDto dto)
        {
            var result = await _sMSService.SendSMSAsync(dto.PhoneNumber, dto.Body);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }
    }
}
