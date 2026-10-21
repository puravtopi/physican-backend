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
        private readonly IEmailService _emailService;

        public InquiryController(IInquiryRepository inquiryRepository, IEmailService emailService)
        {
            _inquiryRepository = inquiryRepository;
            _emailService = emailService;
        }


        [HttpPost("AddInquiry")]
        public async Task<IActionResult> AddInquiry([FromBody] InquiryModal modal)
        {
            await _inquiryRepository.SaveInquiry(modal);
            var success = await _emailService.SendEmailAsync(modal.Name, "purav.topi@gmail.com", modal.Note);
            return Ok(1);
        }
    }
}
