using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Implementation;
using PhysicianBooking.Services.Interface;
using System.Threading.Tasks;

namespace PhysicianBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhysioController : ControllerBase
    {
        private readonly IPlaceRepository _placeRepository;

        public PhysioController(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        [HttpGet("GetPhysicians")]
        public async Task<ActionResult<PagedResult<PhysicianModal>>> GetPhysicians(
            [FromQuery] string search = "",
            [FromQuery] string category = "",
            [FromQuery] string zipCode = "",
            [FromQuery] string city = "",
            [FromQuery] string state = "",
            [FromQuery] decimal? minRating = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _placeRepository.GetAllPhysicianAsync(search, category, zipCode, city, state, minRating, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("GetPhysiciansById")]
        public async Task<ActionResult<PhysicianModal>> GetPhysiciansById(
                      [FromQuery] int Id = 1
            )
        {
            var result = await _placeRepository.GetPhysicianByIdAsync(Id);
            return Ok(result);
        }

        [HttpGet("GetPhysiciansReviews")]
        public async Task<ActionResult<PagedResult<ReviewModal>>> GetPhysiciansReviews(
            [FromQuery] int Id = 1,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _placeRepository.GetPhysicianReviewsAsync(Id, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("GetPhysiciansWorkingHours")]
        public async Task<ActionResult<List<string>>> GetPhysiciansWorkingHours(
            [FromQuery] Int64 Id = 1)
           
        {
            var result = await _placeRepository.GetPhysicianWorkingHours(Id);
            return Ok(result);
        }
    }
}
