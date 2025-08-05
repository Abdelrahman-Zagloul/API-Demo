//using Microsoft.AspNetCore.Mvc;

//namespace API_Demo.Controllers
//{
//    [ApiController]
//    [Route("[Controller]")]
//    public class LoggerController : ControllerBase
//    {
//        private readonly ILogger<LoggerController> _logger;

//        public LoggerController(ILogger<LoggerController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        public IActionResult Test()
//        {
//            _logger.LogTrace("LogTrace");
//            _logger.LogDebug("LogDebug");
//            _logger.LogInformation("LogInformation");
//            _logger.LogWarning("LogWarning");
//            _logger.LogError("LogError");
//            _logger.LogCritical("LogCritical");
//            return Ok();
//        }
//    }
//}
