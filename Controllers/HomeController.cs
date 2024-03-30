using HWASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HWASP.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Registration(RegistrationFormModel? formModel)
        {
            RegistrationPageModel model = new()
            {
                PageTitle = "Registration",
                FormModel = formModel != null ? formModel : new RegistrationFormModel()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
