using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;

namespace PhysicianBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {

        private readonly ISpecialtiesRepository _specialtiesRepository;

        public SpecialtiesController(ISpecialtiesRepository specialtiesRepository)
        {
            _specialtiesRepository = specialtiesRepository;
        }

        [HttpGet("GetSpecialties")]
        public async Task<ActionResult<PagedResult<SpecialtiesModal>>> GetSpecialties(
            [FromQuery] string searchText = "",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _specialtiesRepository.GetSpecialtiesAsync(searchText, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("GetZips")]
        public async Task<ActionResult<PagedResult<SpecialtiesModal>>> GetZips(
           [FromQuery] string searchText = "",
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
        {
            var result = await _specialtiesRepository.GetZips(searchText, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("GetCities")]
        public async Task<ActionResult<PagedResult<string>>> GetCities(
           [FromQuery] string searchText = "",
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
        {
            var result = await _specialtiesRepository.GetCities(searchText, pageNumber, pageSize);
            return Ok(result);
        }


        [HttpGet("GetStates")]
        public async Task<ActionResult<PagedResult<string>>> GetStates(
           [FromQuery] string searchText = "",
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
        {
            var result = await _specialtiesRepository.GetStates(searchText, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
