using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain
{
    public class GooglePlacesResponse
    {
        public List<Place> Places { get; set; }

        public string NextPageToken { get; set; }
    }

    public class Place
    {
        public string Id { get; set; }
        public DisplayName DisplayName { get; set; }
        public string FormattedAddress { get; set; }

        [JsonPropertyName("addressComponents")]
        public List<AddressComponent> AddressComponents { get; set; }
        public string NationalPhoneNumber { get; set; }
        public string EditorialSummary { get; set; }
        public string WebsiteUri { get; set; }
        public string GoogleMapsUri { get; set; }
        public double? Rating { get; set; }
        public double? UserRatingCount { get; set; }
        public List<Review> Reviews { get; set; }
        public List<PlacePhoto> Photos { get; set; }
        public Location? Location { get; set; }

        [JsonPropertyName("regularOpeningHours")]
        public OpeningHours? RegularOpeningHours { get; set; }
    }

    public class AddressComponent
    {
        [JsonPropertyName("longText")]
        public string LongText { get; set; }

        [JsonPropertyName("shortText")]
        public string ShortText { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }

        [JsonPropertyName("languageCode")]
        public string LanguageCode { get; set; }
    }

    public class DisplayName
    {
        public string Text { get; set; }
    }

    public class Review
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string RelativePublishTimeDescription { get; set; }
        public int Rating { get; set; }
        public ReviewText Text { get; set; }
        public ReviewText OriginalText { get; set; }
        public AuthorAttribution AuthorAttribution { get; set; }
        public DateTime PublishTime { get; set; }
        public string FlagContentUri { get; set; }
        public string GoogleMapsUri { get; set; }
    }

    public class ReviewText
    {
        public string Text { get; set; }
        public string LanguageCode { get; set; }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class AuthorAttribution
    {
        public string DisplayName { get; set; }
        public string Uri { get; set; }
        public string PhotoUri { get; set; }
    }

    public class PlacePhoto
    {
        public string Name { get; set; }
        public int WidthPx { get; set; }
        public int HeightPx { get; set; }
        public List<AuthorAttribution> AuthorAttributions { get; set; }
        public string FlagContentUri { get; set; }
        public string GoogleMapsUri { get; set; }
    }

    public class OpeningHours
    {
        [JsonPropertyName("weekdayDescriptions")]
        public List<string> WeekdayDescriptions { get; set; } = new();

        [JsonPropertyName("openNow")]
        public bool? OpenNow { get; set; }
    }
}
