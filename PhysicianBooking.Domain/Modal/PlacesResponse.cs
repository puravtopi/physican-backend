using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PlacesResponse
{
    [JsonPropertyName("places")]
    public List<Place> Places { get; set; }
}

public class Place
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("types")]
    public List<string> Types { get; set; }

    [JsonPropertyName("nationalPhoneNumber")]
    public string NationalPhoneNumber { get; set; }

    [JsonPropertyName("internationalPhoneNumber")]
    public string InternationalPhoneNumber { get; set; }

    [JsonPropertyName("formattedAddress")]
    public string FormattedAddress { get; set; }

    [JsonPropertyName("addressComponents")]
    public List<AddressComponent> AddressComponents { get; set; }

    [JsonPropertyName("plusCode")]
    public PlusCode PlusCode { get; set; }

    [JsonPropertyName("location")]
    public LatLng Location { get; set; }

    [JsonPropertyName("viewport")]
    public Viewport Viewport { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("googleMapsUri")]
    public string GoogleMapsUri { get; set; }

    [JsonPropertyName("websiteUri")]
    public string WebsiteUri { get; set; }

    [JsonPropertyName("regularOpeningHours")]
    public OpeningHours RegularOpeningHours { get; set; }

    [JsonPropertyName("utcOffsetMinutes")]
    public int UtcOffsetMinutes { get; set; }

    [JsonPropertyName("adrFormatAddress")]
    public string AdrFormatAddress { get; set; }

    [JsonPropertyName("businessStatus")]
    public string BusinessStatus { get; set; }

    [JsonPropertyName("userRatingCount")]
    public int UserRatingCount { get; set; }

    [JsonPropertyName("displayName")]
    public LocalizedText DisplayName { get; set; }

    [JsonPropertyName("primaryTypeDisplayName")]
    public LocalizedText PrimaryTypeDisplayName { get; set; }

    [JsonPropertyName("currentOpeningHours")]
    public OpeningHours CurrentOpeningHours { get; set; }

    [JsonPropertyName("primaryType")]
    public string PrimaryType { get; set; }

    [JsonPropertyName("shortFormattedAddress")]
    public string ShortFormattedAddress { get; set; }

    [JsonPropertyName("reviews")]
    public List<Review> Reviews { get; set; }

    [JsonPropertyName("photos")]
    public List<Photo> Photos { get; set; }

    [JsonPropertyName("restroom")]
    public bool Restroom { get; set; }

    [JsonPropertyName("paymentOptions")]
    public PaymentOptions PaymentOptions { get; set; }

    [JsonPropertyName("parkingOptions")]
    public ParkingOptions ParkingOptions { get; set; }

    [JsonPropertyName("accessibilityOptions")]
    public AccessibilityOptions AccessibilityOptions { get; set; }

    [JsonPropertyName("addressDescriptor")]
    public AddressDescriptor AddressDescriptor { get; set; }

    [JsonPropertyName("googleMapsLinks")]
    public GoogleMapsLinks GoogleMapsLinks { get; set; }

    [JsonPropertyName("timeZone")]
    public TimeZoneInfoData TimeZone { get; set; }

    [JsonPropertyName("postalAddress")]
    public PostalAddress PostalAddress { get; set; }
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

public class PlusCode
{
    [JsonPropertyName("globalCode")]
    public string GlobalCode { get; set; }

    [JsonPropertyName("compoundCode")]
    public string CompoundCode { get; set; }
}

public class LatLng
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}

public class Viewport
{
    [JsonPropertyName("low")]
    public LatLng Low { get; set; }

    [JsonPropertyName("high")]
    public LatLng High { get; set; }
}

public class OpeningHours
{
    [JsonPropertyName("openNow")]
    public bool OpenNow { get; set; }

    [JsonPropertyName("periods")]
    public List<Period> Periods { get; set; }

    [JsonPropertyName("weekdayDescriptions")]
    public List<string> WeekdayDescriptions { get; set; }

    [JsonPropertyName("nextOpenTime")]
    public DateTimeOffset? NextOpenTime { get; set; }
}

public class Period
{
    [JsonPropertyName("open")]
    public PointInTime Open { get; set; }

    [JsonPropertyName("close")]
    public PointInTime Close { get; set; }
}

public class PointInTime
{
    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("hour")]
    public int Hour { get; set; }

    [JsonPropertyName("minute")]
    public int Minute { get; set; }

    [JsonPropertyName("date")]
    public Date Date { get; set; }
}

public class Date
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("day")]
    public int Day { get; set; }
}

public class LocalizedText
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("languageCode")]
    public string LanguageCode { get; set; }
}

public class Review
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("relativePublishTimeDescription")]
    public string RelativePublishTimeDescription { get; set; }

    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    [JsonPropertyName("text")]
    public LocalizedText Text { get; set; }

    [JsonPropertyName("authorAttribution")]
    public AuthorAttribution AuthorAttribution { get; set; }

    [JsonPropertyName("publishTime")]
    public DateTime PublishTime { get; set; }
}

public class AuthorAttribution
{
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }

    [JsonPropertyName("photoUri")]
    public string PhotoUri { get; set; }
}

public class Photo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("widthPx")]
    public int WidthPx { get; set; }

    [JsonPropertyName("heightPx")]
    public int HeightPx { get; set; }

    [JsonPropertyName("authorAttributions")]
    public List<AuthorAttribution> AuthorAttributions { get; set; }
}

public class PaymentOptions
{
    [JsonPropertyName("acceptsCreditCards")]
    public bool AcceptsCreditCards { get; set; }

    [JsonPropertyName("acceptsDebitCards")]
    public bool AcceptsDebitCards { get; set; }

    [JsonPropertyName("acceptsNfc")]
    public bool AcceptsNfc { get; set; }
}

public class ParkingOptions
{
    [JsonPropertyName("freeParkingLot")]
    public bool FreeParkingLot { get; set; }
}

public class AccessibilityOptions
{
    [JsonPropertyName("wheelchairAccessibleParking")]
    public bool WheelchairAccessibleParking { get; set; }

    [JsonPropertyName("wheelchairAccessibleEntrance")]
    public bool WheelchairAccessibleEntrance { get; set; }

    [JsonPropertyName("wheelchairAccessibleRestroom")]
    public bool WheelchairAccessibleRestroom { get; set; }

    [JsonPropertyName("wheelchairAccessibleSeating")]
    public bool WheelchairAccessibleSeating { get; set; }
}

public class AddressDescriptor
{
    [JsonPropertyName("landmarks")]
    public List<Landmark> Landmarks { get; set; }
}

public class Landmark
{
    [JsonPropertyName("displayName")]
    public LocalizedText DisplayName { get; set; }

    [JsonPropertyName("straightLineDistanceMeters")]
    public double StraightLineDistanceMeters { get; set; }

    [JsonPropertyName("spatialRelationship")]
    public string SpatialRelationship { get; set; }
}

public class GoogleMapsLinks
{
    [JsonPropertyName("directionsUri")]
    public string DirectionsUri { get; set; }

    [JsonPropertyName("placeUri")]
    public string PlaceUri { get; set; }
}

public class TimeZoneInfoData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class PostalAddress
{
    [JsonPropertyName("regionCode")]
    public string RegionCode { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("administrativeArea")]
    public string AdministrativeArea { get; set; }

    [JsonPropertyName("locality")]
    public string Locality { get; set; }

    [JsonPropertyName("addressLines")]
    public List<string> AddressLines { get; set; }
}