using Microsoft.AspNetCore.Mvc;

namespace emaill.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMailService _mailService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> SendEmail(string toEmail, string subject, string body)
        {
            await _mailService.SendEmailAsync("nugzari.rostiashvili.1@btu.edu.ge", "hello", "asdfasdasd");
            return Ok("Email sent successfully");
        }
    }
}
