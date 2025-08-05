//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using API_Demo.Configuration;
//namespace API_Demo.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class TestConfigurationController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly ConnectionStringsOptions _connectionStringsOptions;
//        private readonly IOptions<ConnectionStringsOptions> _options;
//        private readonly IOptionsSnapshot<ConnectionStringsOptions> _optionsSnapshot;
//        private readonly IOptionsMonitor<ConnectionStringsOptions> _optionsMonitor;
//        public TestConfigurationController(
//            IConfiguration configuration,
//            ConnectionStringsOptions connectionStringsOptions,
//            IOptions<ConnectionStringsOptions> options,
//            IOptionsSnapshot<ConnectionStringsOptions> optionsSnapshot,
//            IOptionsMonitor<ConnectionStringsOptions> optionsMonitor)

//        {
//            _configuration = configuration;
//            _connectionStringsOptions = connectionStringsOptions;
//            _options = options;
//            _optionsSnapshot = optionsSnapshot;
//            _optionsMonitor = optionsMonitor;
//        }
//        [HttpGet]
//        public IActionResult GetInfo()
//        {
//            var x = new
//            {
//                IConfiguration = _configuration["ConnectionStrings:DefaultConnect"],
//                ConnectionStringsOptions = _connectionStringsOptions.DefaultConnect,
//                IOptions = _options.Value.DefaultConnect,
//                IOptionsSnapshot= _optionsSnapshot.Value.DefaultConnect,
//                IOptionsMonitor = _optionsMonitor.CurrentValue.DefaultConnect,

//            };
//            return Ok(x);
//        }

//    }
//}