using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

namespace emaill

{
    public class MailService : IMailService
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly HttpClient _httpClient;

        public MailService(IConfiguration configuration)
        {
            _apiKey = configuration["Mailjet:ApiKey"];
            _apiSecret = configuration["Mailjet:ApiSecret"];
            _fromEmail = configuration["Mailjet:FromEmail"];
            _fromName = configuration["Mailjet:FromName"];
            _httpClient = new HttpClient();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var requestUri = "https://api.mailjet.com/v3.1/send";
            var requestContent = new JObject
            {
                ["Messages"] = new JArray
            {
                new JObject
                {
                    ["From"] = new JObject
                    {
                        ["Email"] = _fromEmail,
                        ["Name"] = _fromName
                    },
                    ["To"] = new JArray
                    {
                        new JObject
                        {
                            ["Email"] = toEmail,
                            ["Name"] = "Recipient Name"
                        }
                    },
                    ["Subject"] = subject,
                    ["TextPart"] = body,
                    ["HTMLPart"] = $"<p>{body}</p>"
                }
            }
            };

            var byteArray = Encoding.ASCII.GetBytes($"{_apiKey}:{_apiSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = await _httpClient.PostAsync(requestUri, new StringContent(requestContent.ToString(), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Mailjet API call failed: {responseBody}");
            }
        }
    }

}
