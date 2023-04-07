using ECommer.DAL.Entities;

namespace ECommer.DAL
{
	public class SeederBD
	{
		#region Builder
		private readonly DataBaseContext _context;

		public SeederBD(DataBaseContext context)
		{
			_context = context;
		}
		#endregion

		#region Public methods
		public async Task SeedAsync()
		{
			await _context.Database.EnsureCreatedAsync();

			await PopulateCategoriesAsync();
			await PopulateCountiesStatesCitiesAsync();

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
		#endregion
	}
}
