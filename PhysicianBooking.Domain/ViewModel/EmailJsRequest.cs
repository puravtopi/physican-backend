using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain.ViewModel
{
    public class EmailJsRequest
    {
        [JsonPropertyName("service_id")]
        public string ServiceId { get; set; } = string.Empty;

        [JsonPropertyName("template_id")]
        public string TemplateId { get; set; } = string.Empty;

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty; // EmailJS Public Key

        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; } // EmailJS Private Key

        [JsonPropertyName("template_params")]
        public Dictionary<string, string> TemplateParams { get; set; } = new();
    }

    public class SendContactEmailDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
