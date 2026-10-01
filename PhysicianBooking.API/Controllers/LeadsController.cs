using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;

namespace PhysicianBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadsRepository _leadsRepository;

        public LeadsController(ILeadsRepository leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }


        [HttpPost("AddLeads")]
        public async Task<IActionResult> AddLeads([FromBody] LeadsModal modal)
        {
            await _leadsRepository.SaveLeads(modal);
            return Ok(1);  
        }
    }   
}
