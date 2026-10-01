using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace PhysicianBooking.Services.Interface
{
    public interface IPlaceRepository
    {
        Task SavePlace(dynamic result);
        Task<PagedResult<PhysicianModal>> GetAllPhysicianAsync(string search = "", string category = "", string zipcode = "",
            string city = "",
            string state = "",
             decimal? minRating = null, int pageNumber = 1, int pageSize = 10);
        Task<PhysicianModal> GetPhysicianByIdAsync(int Id);
        Task<PagedResult<ReviewModal>> GetPhysicianReviewsAsync(int Id, int pageNumber = 1, int pageSize = 10);
        Task<List<string>> GetPhysicianWorkingHours(Int64 Id);
    }
}