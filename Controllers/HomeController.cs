using ASP1.Services.Kdf;
using HWASP.Data.DAL;
using HWASP.Models;
using HWASP.Services.RandomServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace HWASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRandomService _rndService;

        private readonly DataAccessor _dataAccessor;

        private readonly IRandomService _randomService;
        private readonly IKdfService _kdfService;
        public HomeController(ILogger<HomeController> logger, IRandomService rndService, DataAccessor dataAccessor, IRandomService randomService, IKdfService kdfService)
        {
            _logger = logger;
            _rndService = rndService;
            _dataAccessor = dataAccessor;
            _randomService = randomService;
            _kdfService = kdfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ViewResult Registration(RegistrationFormModel? formModel)
        {
            RegistrationPageModel model = new()
            {
                FormModel = formModel,
                ValidationErrors=_ValidateRegistrationModel(formModel),
                IsInitialLoad = true

            };
            if (formModel != null && formModel?.UserEmail != null) {
                if(model.ValidationErrors.Count > 0)
                {
                    model.Message = "Регистрация отклонена.";
                    model.IsSuccess = false;
                    model.IsInitialLoad = false;
                }
                else
                {
                    _dataAccessor.UserDao.RegisterUser(mapUser(formModel));
                    model.Message = "Вы успешно зарегистрированы";
                    model.IsSuccess = true;
                }
            }
            else if(String.IsNullOrEmpty(model.Message)) 
            {
                model.Message = "Заполните обязательные поля формы";
            }
           
            return View(model);
        }

        private Dictionary<String,String> _ValidateRegistrationModel(RegistrationFormModel? model)
        {
            Dictionary<String, String> res = new();
            if (model == null)
            {
                res[nameof(model)] = "Model is null";
            }
            else
            {
                if(String.IsNullOrEmpty(model.UserName) && String.IsNullOrEmpty(model.UserEmail)
                    && String.IsNullOrEmpty(model.Password))
                {
                    res[nameof(model.UserName)] = "At least one field should be filled";
                }
                if (String.IsNullOrEmpty(model.UserName))
                {
                    res[nameof(model.UserName)] = "UserName is empty";
                }
                if (String.IsNullOrEmpty(model.UserEmail))
                {
                    res[nameof(model.UserEmail)] = "Email is empty";
                }
                if (_dataAccessor.UserDao.IsEmailFree(model.UserEmail)) {
                    res[nameof(model.UserEmail)] = "Email in use";
                }
                if(String.IsNullOrEmpty(model.Password))
                {
                    res[nameof(model.Password)] = "Password is empty";
                }
                if(model.Password!=model.PasswordRepeat)
                {
                    res[nameof(model.PasswordRepeat)] = "Passwords are not equal";
                }
                if (!model.Confirm)
                {
                    res[nameof(model.Confirm)] = "Confirm expeced";
                }
            }
            return res;
        }

        private Data.Entities.User mapUser(RegistrationFormModel formModel)
        {
            String salt = _randomService.GenerateCryptoSalt();
            Data.Entities.User user = new()
            {
                Id = Guid.NewGuid(),
                Name = formModel.UserName,
                Email = formModel.UserEmail,
                Password = formModel.Password,
                Birthdate = formModel.Birthdate,
                Register = DateTime.Now,
                Salt = salt,
                DerivedKey = _kdfService.GetDerivedKey(formModel.Password, salt)
            };
            return user;
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
