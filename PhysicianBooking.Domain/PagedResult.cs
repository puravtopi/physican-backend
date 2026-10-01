using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Domain
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();

        public IEnumerable<T> AllData { get; set; } = new List<T>();
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public int CurrentPage { get; set; }
    }
}
