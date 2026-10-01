using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain.Modal
{
    public class PlaceDetailsResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("displayName")]
        public DisplayName? DisplayName { get; set; }

        [JsonPropertyName("regularOpeningHours")]
        public RegularOpeningHours? RegularOpeningHours { get; set; }
    }

    public class DisplayName
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; }
    }

    public class RegularOpeningHours
    {
        [JsonPropertyName("openNow")]
        public bool? OpenNow { get; set; }

        [JsonPropertyName("weekdayDescriptions")]
        public List<string> WeekdayDescriptions { get; set; } = new();
    }
}
