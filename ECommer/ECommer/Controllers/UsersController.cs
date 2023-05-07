using ECommer.DAL;
using ECommer.DAL.Entities;
using ECommer.Helpers;
using ECommer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommer.Controllers
{
    public class UsersController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IUserHelpers _userHelpers;
        private readonly IDropDownListHelper _dropDownListHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public UsersController(DataBaseContext context, IUserHelpers userHelpers, IDropDownListHelper dropDownListHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _userHelpers = userHelpers;
            _dropDownListHelper = dropDownListHelper;
            _azureBlobHelper = azureBlobHelper;
        }
        #endregion

        #region User actions
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                       View(await _context.Users
                       .Include(u => u.City)
                       .ThenInclude(c => c.State)
                       .ThenInclude(s => s.Country)
                       .ToListAsync()) :
                       Problem("Entity set 'DataBaseContext.Users'  is null.");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                Countries = await _dropDownListHelper.GetDDLCountriesAsync(),
                States = await _dropDownListHelper.GetDDLStatesAsync(new Guid()),
                Cities = await _dropDownListHelper.GetDDLCitiesAsync(new Guid()),
                UserType = Enum.UserType.Admin
            };
            return View(addUserViewModel);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(AddUserViewModel addUserViewModel)
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
                return RedirectToAction("Index", "Users");
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

            return Json(country.States.OrderBy(c => c.Name));
        }

        [HttpGet]
        public JsonResult GetCities(Guid stateId)
        {
            State state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id.Equals(stateId));

            if (state == null) return null;

            return Json(state.Cities.OrderBy(s => s.Name));
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
