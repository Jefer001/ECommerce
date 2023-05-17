using ECommer.DAL;
using ECommer.DAL.Entities;
using ECommer.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommer.Services
{
    public class DropDownListHelper : IDropDownListHelper
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public DropDownListHelper(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public methods
        public async Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync()
        {
            List<SelectListItem> listCategories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCategories.Insert(0, new SelectListItem
            {
                Text = "Seleccione una categoía...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listCategories;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync(IEnumerable<Category> filterCategories)
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Category> categoriesFiltered = new();

            foreach (Category category in categories)
                if (!filterCategories.Any(c => c.Id.Equals(category.Id)))
                    categoriesFiltered.Add(category);

            List<SelectListItem> ListCategories = categoriesFiltered
                .Select(c => new SelectListItem 
                { 
                    Text = c.Name, Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToList();

            ListCategories.Insert(0, new SelectListItem
            {
                Text = "Seleccione una categoría...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return ListCategories;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCountriesAsync()
        {
            List<SelectListItem> listCountries = await _context.Countries
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCountries.Insert(0, new SelectListItem
            {
                Text = "Seleccione un país...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listCountries;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLStatesAsync(Guid countryId)
        {
            List<SelectListItem> listStates = await _context.States
                .Where(s => s.Country.Id == countryId)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .OrderBy(s => s.Text)
                .ToListAsync();

            listStates.Insert(0, new SelectListItem
            {
                Text = "Seleccione un estado...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listStates;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLCitiesAsync(Guid stateId)
        {
            List<SelectListItem> listCities = await _context.Cities
                .Where(c => c.State.Id == stateId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listCities.Insert(0, new SelectListItem
            {
                Text = "Seleccione una cuidad...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listCities;
        }
        #endregion
    }
}
