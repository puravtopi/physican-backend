using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain.Modal
{
    public class InquiryModal
    {
        public long? Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
}
