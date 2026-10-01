using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;

namespace PhysicianBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryController : Controller
    {
        private readonly IInquiryRepository _inquiryRepository;

        public InquiryController(IInquiryRepository inquiryRepository)
        {
            _inquiryRepository = inquiryRepository;
        }


        [HttpPost("AddInquiry")]
        public async Task<IActionResult> AddInquiry([FromBody] InquiryModal modal)
        {
            await _inquiryRepository.SaveInquiry(modal);
            return Ok(1);
        }
    }
}
