using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain.Modal
{
    public class ReviewModal
    {
        public Int64 Id { get; set; }
        public Int64 PlaceId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public Int16 Rating { get; set; }
        public DateOnly Date { get; set; }
    }
}
