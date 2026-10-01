using PhysicianBooking.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Interface
{
    public interface IGooglePlacesService
    {
        Task<string> SearchPlaces(string query);
        Task<GooglePlacesResponse> SearchAndSaveBusinesses(string category, string zipCode);

        Task<bool> GetWeekdayDescriptionsAsync();
    }
}
