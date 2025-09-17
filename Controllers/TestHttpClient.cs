//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
//using static System.Net.WebRequestMethods;

//namespace API_Demo.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TestHttpClient : ControllerBase
//    {
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            string url = "https://open.er-api.com/v6/latest/USD";
//            using var client = new HttpClient();

//            var response = await client.GetAsync(url);

//            if (!response.IsSuccessStatusCode)
//            {
//                return StatusCode((int)response.StatusCode, "Error fetching data");
//            }
//            // 4- نقرأ البيانات كنص
//            string json = await response.Content.ReadAsStringAsync();

//            var options = new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            };

//            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json, options);

//            return Ok(data);
//        }

//        [HttpGet("translate")]
//        public async Task<IActionResult> TranslateToArabic(string text = "Welcome")
//        {
//            var url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair=en|ar";

//            using var client = new HttpClient();
//            var response = await client.GetStringAsync(url);

//            using var doc = JsonDocument.Parse(response);
//            var translated = doc.RootElement
//                                .GetProperty("responseData")
//                                .GetProperty("translatedText")
//                                .GetString();

//            return Ok(new { Original = text, Arabic = translated });
//        }

//    }
//    public class ExchangeRateResponse
//    {
//        public string Result { get; set; }
//        public string BaseCode { get; set; }
//        public Dictionary<string, decimal> Rates { get; set; }
//    }
//}
