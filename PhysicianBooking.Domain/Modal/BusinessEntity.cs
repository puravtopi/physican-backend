namespace YourProject.Models;

public class BusinessEntity
{
    // Identification
    public string ExternalId { get; set; } // Google's Place ID
    public string Name { get; set; }
    public string PrimaryCategory { get; set; }

    // Contact & Web
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
    public string GoogleMapsUrl { get; set; }

    // Reputation
    public double Rating { get; set; }
    public int TotalReviews { get; set; }

    // Status & Location
    public bool IsOpenNow { get; set; }
    public string BusinessStatus { get; set; } // e.g., OPERATIONAL
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Branding (Optional)
    public string IconUrl { get; set; }
    public string IconBackgroundColor { get; set; }
}