using ECommer.DAL.Entities;
using ECommer.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommer.Helpers
{
	public interface IUserHelpers
	{
		Task<User> GetUserAsync(string email);

		Task<IdentityResult> AddUserAsync(User user, string password);

		Task AddRoleAsync(string roleName);

		Task AddUserToRoleAsync(User user, string roleName);

		Task<bool> IsUserInRoleAsync(User user, string roleName);

		Task<SignInResult> LoginAsync(LoginViewModel loginViewModel);

		Task LogoutAsync();
	}
}
