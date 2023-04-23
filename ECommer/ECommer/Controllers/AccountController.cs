using ECommer.Helpers;
using ECommer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommer.Controllers
{
	public class AccountController : Controller
	{
		#region Constants
		private readonly IUserHelpers _userHelpers;
		#endregion

		#region Builder
		public AccountController(IUserHelpers userHelpers)
        {
            _userHelpers = userHelpers;
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
		#endregion
	}    
}
