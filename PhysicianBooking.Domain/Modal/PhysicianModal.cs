using System.Collections.Generic;

namespace PhysicianBooking.Domain.Modal
{

    public class PhysicianModal
    {
        public int Id { get; set; } 
        public string PlaceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public Int64? ZipCode { get; set; }
        public string Phone { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public string Hospital { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public int Experience { get; set; } = 0;
        public string Bio { get; set; } = string.Empty;
    }
}
