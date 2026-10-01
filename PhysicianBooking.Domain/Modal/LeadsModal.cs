using PhysicianBooking.Domain.ViewModel;

namespace PhysicianBooking.Domain.Modal
{
    public class LeadsModal
    {
        public Int64 Id { get; set; }
        public Int64 PlaceId { get; set; }
        public string Place { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string IP_Address { get; set; } = string.Empty;
        public DateOnly CreatedDate { get; set; }
    }
}
