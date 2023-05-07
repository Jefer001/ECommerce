using ECommer.DAL;
using ECommer.DAL.Entities;
using ECommer.Enum;
using ECommer.Helpers;
using ECommer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommer.Controllers
{
    public class AccountController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IUserHelpers _userHelpers;
        private readonly IDropDownListHelper _dropDownListHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public AccountController(IUserHelpers userHelpers, DataBaseContext context, IDropDownListHelper dropDownListHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _userHelpers = userHelpers;
            _dropDownListHelper = dropDownListHelper;
            _azureBlobHelper = azureBlobHelper;
        }
        #endregion

        #region Login actions
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelpers.LoginAsync(loginViewModel);

                if (result.Succeeded) return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelpers.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                Countries = await _dropDownListHelper.GetDDLCountriesAsync(),
                States = await _dropDownListHelper.GetDDLStatesAsync(new Guid()),
                Cities = await _dropDownListHelper.GetDDLCitiesAsync(new Guid()),
                UserType = UserType.User,
            };

            return View(addUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (addUserViewModel.ImageFile != null)
                    imageId = await _azureBlobHelper.UploadAzureBlobAsync(addUserViewModel.ImageFile, "users");

                addUserViewModel.ImageId = imageId;
                addUserViewModel.CreatedDate = DateTime.Now;

                User user = await _userHelpers.AddUserAsync(addUserViewModel);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    await FillDropDownListLocation(addUserViewModel);

                    return View(addUserViewModel);
                }

                //Autologeamos al nuevo usuario que se registra
                LoginViewModel loginViewModel = new()
                {
                    Password = addUserViewModel.Password,
                    Username = addUserViewModel.Username,
                    RememberMe = false
                };

                var login = await _userHelpers.LoginAsync(loginViewModel);

                if (login.Succeeded) return RedirectToAction("Index", "Home");
            }
            await FillDropDownListLocation(addUserViewModel);

            return View(addUserViewModel);
        }

        [HttpGet]
        public JsonResult GetStates(Guid countryId)
        {
            Country country = _context.Countries
                .Include(c => c.States)
                .FirstOrDefault(c => c.Id.Equals(countryId));

            if (country == null) return null;

            return Json(country.States.OrderBy(d => d.Name));
        }

        [HttpGet]
        public JsonResult GetCities(Guid stateId)
        {
            State state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id.Equals(stateId));

            if (state == null) return null;

            return Json(state.Cities.OrderBy(c => c.Name));
        }
        #endregion

        #region Private methods
        private async Task FillDropDownListLocation(AddUserViewModel addUserViewModel)
        {
            addUserViewModel.Countries = await _dropDownListHelper.GetDDLCountriesAsync();
            addUserViewModel.States = await _dropDownListHelper.GetDDLStatesAsync(addUserViewModel.CountryId);
            addUserViewModel.Cities = await _dropDownListHelper.GetDDLCitiesAsync(addUserViewModel.StateId);
        }
        #endregion
    }
}
