using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain.ViewModel
{
    public class BaseModel
    {
        public int? TotalCount { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public int? CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }
        public int? TotalPages => (int)Math.Ceiling((double)TotalCount.Value / PageSize.Value);

    }
}
