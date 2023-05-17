using ECommer.DAL.Entities;
using ECommer.Enum;
using ECommer.Helpers;
using ECommer.Services;

namespace ECommer.DAL
{
	public class SeederBD
	{
		#region Constants
		private readonly DataBaseContext _context;
		private readonly IUserHelpers _userHelpers;
        private readonly IAzureBlobHelper _azureBlobHelper;
        #endregion

        #region Builder
        public SeederBD(DataBaseContext context, IUserHelpers userHelpers, IAzureBlobHelper azureBlobHelper)
		{
			_context = context;
			_userHelpers = userHelpers;
			_azureBlobHelper = azureBlobHelper;
		}
		#endregion

		#region Public methods
		public async Task SeedAsync()
		{
			await _context.Database.EnsureCreatedAsync();

			await PopulateCategoriesAsync();
			await PopulateCountiesStatesCitiesAsync();
			await PopulateRolesAsync();
            await PopulateUserAsync("Steve", "Jobs", "steve_jobs_admin@yopmail.com", "3002323232", "Street Apple", "102030", "admin_use.png", UserType.Admin);
            await PopulateUserAsync("Bill", "Gates", "bill_gates_user@yopmail.com", "4005656656", "Street Microsoft", "405060", "user.png", UserType.User);

            await _context.SaveChangesAsync();
		}
		#endregion

		#region Private methods
		private async Task PopulateCategoriesAsync()
		{
			if (!_context.Categories.Any())
			{
				_context.Categories.Add(new Category { Name = "Tecnología", Description = "Elementos tech", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Implementos de aseo", Description = "Detergente, jabón, etc.", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Ropa interior", Description = "Tanguitas narizonas", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Gamers", Description = "PS5, XBOX Series", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Mascotas", Description = "Concentrado, Jabón para pulgas.", CreatedDate = DateTime.Now });
			}
		}

		private async Task PopulateCountiesStatesCitiesAsync()
		{
			if (!_context.Countries.Any())
			{
				_context.Countries.Add(
				new Country
				{
					Name = "Colombia",
					CreatedDate = DateTime.Now,
					States = new List<State>()
					{
						new State
						{
							Name = "Antioquia",
							CreatedDate = DateTime.Now,
							Cities = new List<City>() 
							{
								new City { Name = "Medellín", CreatedDate= DateTime.Now },
								new City { Name = "Bello", CreatedDate= DateTime.Now },
								new City { Name = "Copacabana", CreatedDate= DateTime.Now },
								new City { Name = "La Estrella", CreatedDate= DateTime.Now },
								new City { Name = "Sabaneta", CreatedDate= DateTime.Now },
								new City { Name = "Envigado", CreatedDate= DateTime.Now },
							}
						},
						new State
						{
							Name = "Cundinamarca",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Bogotá", CreatedDate= DateTime.Now },
								new City { Name = "Fusagasugá", CreatedDate= DateTime.Now },
								new City { Name = "Funza", CreatedDate= DateTime.Now },
								new City { Name = "Sopó", CreatedDate= DateTime.Now },
								new City { Name = "Chía", CreatedDate= DateTime.Now },
							}
						},
						new State
						{
							Name = "Atlántico",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Barranquilla", CreatedDate= DateTime.Now },
								new City { Name = "La Chinita", CreatedDate= DateTime.Now },
							}
						}
					}
				});
			}
		}

		private async Task PopulateRolesAsync()
		{
			await _userHelpers.AddRoleAsync(UserType.Admin.ToString());
			await _userHelpers.AddRoleAsync(UserType.User.ToString());
		}

		private async Task PopulateUserAsync(string firstName, string lastName, string email, string phone, string address, string document, string image, UserType userType)
		{
			User user = await _userHelpers.GetUserAsync(email);

			if (user == null)
			{
                Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
                user = new User
				{
					CreatedDate = DateTime.Now,
					FirstName = firstName,
					LastName = lastName,
					Email = email,
					UserName = email,
					PhoneNumber = phone,
					Address = address,
					Document = document,
					City = _context.Cities.FirstOrDefault(),
					ImageId = imageId,
					UserType = userType
				};
				await _userHelpers.AddUserAsync(user, "123456");
				await _userHelpers.AddUserToRoleAsync(user, userType.ToString());
			}
		}
		#endregion
	}
}
