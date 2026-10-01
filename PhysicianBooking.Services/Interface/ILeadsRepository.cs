using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Interface
{
    public interface ILeadsRepository
    {
        Task SaveLeads(LeadsModal modal);

        Task<PagedResult<LeadsModal>> GetAllLeadsAsync(string search = "",
    int pageNumber = 1,
    string fromDate = "",
    string toDate = "",
    int pageSize = 10);
    }
}
