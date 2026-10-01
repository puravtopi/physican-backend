using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.Services.Implementation;

namespace PhysicianBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GooglePlaceController : ControllerBase
    {
        private readonly GooglePlacesService _service;
        public GooglePlaceController(GooglePlacesService service) => _service = service;

        [HttpGet("search")]
        public async Task<IActionResult> Search(string category, string zip)
        {
            var results = await _service.SearchAndSaveBusinesses(category, zip);
            return Ok(results);
        }

        [HttpGet("searchhours")]
        public async Task<IActionResult> SearchHours()
        {
            var results = await _service.GetWeekdayDescriptionsAsync();
            return Ok(results);
        }
    }
}
