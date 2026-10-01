using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.Services.Interface;

namespace PhysicianBooking.CPanel.Controllers
{
    public class LeadsController : Controller
    {
        private readonly ILeadsRepository _leadsRepository;

        public LeadsController(ILeadsRepository leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }

        public IActionResult Index(string search = "", string fromDate = "", string toDate = "", int page = 1)
        {
            var data = _leadsRepository.GetAllLeadsAsync(search, page, fromDate, toDate).Result;
            var allData = _leadsRepository.GetAllLeadsAsync(search, page, fromDate, toDate, 0).Result;

            ViewBag.SearchText = search;
            ViewBag.FromDate = fromDate == "" ? "" : Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate == "" ? "" : Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");

            data.CurrentPage = page;
            data.SearchTerm = search;
            data.AllData = allData.Data;
            return View(data);
        }
    }
}
