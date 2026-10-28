using Microsoft.Extensions.Configuration;
using PhysicianBooking.Domain.ViewModel;
using PhysicianBooking.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Implementation
{
    public class EmailJsService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string EmailJsApiUrl = "https://api.emailjs.com/api/v1.0/email/send";

        public EmailJsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string name, string toEmail, string message)
        {
            var settings = _configuration.GetSection("EmailJsSettings");

            var payload = new EmailJsRequest
            {
                ServiceId = settings["ServiceId"]!,
                TemplateId = settings["TemplateId"]!,
                UserId = settings["PublicKey"]!,
                AccessToken = settings["PrivateKey"], // Required for backend calls
                TemplateParams = new Dictionary<string, string>
            {
                { "to_email", toEmail },
                { "from_name", name },
                { "message", message }
                // Add any other variables that exist in your EmailJS template
            }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(EmailJsApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"EmailJS Error: {error}");
                return false;
            }

            return true;
        }
    }

}
