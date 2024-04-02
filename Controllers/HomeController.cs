using HWASP.Models;
using HWASP.Services.RandomServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HWASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRandomService _rndService;
        public HomeController(ILogger<HomeController> logger, IRandomService rndService)
        {
            _logger = logger;
            _rndService = rndService;
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

        public ViewResult Random()
        {
            RandomServiceTestPageModel model = new()
            {
                code = _rndService.GenerateConfirmationCode(),
                salt = _rndService.GenerateCryptoSalt(),
                fileName = _rndService.GenerateRandomFileName()
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
