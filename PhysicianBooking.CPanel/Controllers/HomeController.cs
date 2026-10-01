using Microsoft.AspNetCore.Mvc;
using PhysicianBooking.CPanel.Models;
using PhysicianBooking.Domain.ViewModel;
using System.Diagnostics;

namespace PhysicianBooking.CPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (model.Password == "admin" && model.UserName == "admin")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
