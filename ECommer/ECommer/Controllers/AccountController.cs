using ECommer.DAL;
using ECommer.DAL.Entities;
using ECommer.Enum;
using ECommer.Helpers;
using ECommer.Models;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> EditUser()
        {
            User user = await _userHelpers.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound();

            EditUserViewModel editUserViewModel = new()
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Document = user.Document,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Cities = await _dropDownListHelper.GetDDLCitiesAsync(user.City.State.Id),
                CityId = user.City.Id,
                Countries = await _dropDownListHelper.GetDDLCountriesAsync(),
                CountryId = user.City.State.Country.Id,
                States = await _dropDownListHelper.GetDDLStatesAsync(user.City.State.Country.Id),
                StateId = user.City.State.Id,
                Id = Guid.Parse(user.Id)
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = editUserViewModel.ImageId;
                if (editUserViewModel.ImageFile != null) imageId = await _azureBlobHelper.UploadAzureBlobAsync(editUserViewModel.ImageFile, "users");

                User user = await _userHelpers.GetUserAsync(User.Identity.Name);
                user.FirstName = editUserViewModel.FirstName;
                user.LastName = editUserViewModel.LastName;
                user.Document = editUserViewModel.Document;
                user.Address = editUserViewModel.Address;
                user.PhoneNumber = editUserViewModel.PhoneNumber;
                user.ImageId = imageId;
                user.City = await _context.Cities.FindAsync(editUserViewModel.CityId);

                IdentityResult result = await _userHelpers.UpdateUserAsync(user);
                if (result.Succeeded) return RedirectToAction("Index", "Home");
                else ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
            }
            await FillDropDownListLocation(editUserViewModel);
            return View(editUserViewModel);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (changePasswordViewModel.OldPassword.Equals(changePasswordViewModel.NewPassword))
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña diferente");
                    return View(changePasswordViewModel);
                }
                User user = await _userHelpers.GetUserAsync(User.Identity?.Name);
                if (user != null)
                {
                    IdentityResult result = await _userHelpers.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                    if (result.Succeeded) return RedirectToAction("EditUser");
                    else ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
                else ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
            }
            return View(changePasswordViewModel);
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

        private async Task FillDropDownListLocation(EditUserViewModel editUserViewModel)
        {
            editUserViewModel.Countries = await _dropDownListHelper.GetDDLCountriesAsync();
            editUserViewModel.States = await _dropDownListHelper.GetDDLStatesAsync(editUserViewModel.CountryId);
            editUserViewModel.Cities = await _dropDownListHelper.GetDDLCitiesAsync(editUserViewModel.StateId);
        }
        #endregion
    }
}
