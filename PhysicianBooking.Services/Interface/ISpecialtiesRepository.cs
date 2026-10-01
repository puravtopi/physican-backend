using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Interface
{
    public interface ISpecialtiesRepository
    {
        Task<PagedResult<SpecialtiesModal>> GetSpecialtiesAsync(string searchText = "", int pageNumber = 1, int pageSize = 10);

        Task<PagedResult<ZipModal>> GetZips(string searchText, int pageNumber, int pageSize);
        Task<PagedResult<string>> GetCities(string searchText, int pageNumber, int pageSize);
        Task<PagedResult<string>> GetStates(string searchText, int pageNumber, int pageSize);
    }
}
