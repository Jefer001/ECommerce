using ECommer.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommer.Helpers
{
    public interface IDropDownListHelper
    {
        Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetDDLCategoriesAsync(IEnumerable<Category> filterCategories);

        Task<IEnumerable<SelectListItem>> GetDDLCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetDDLStatesAsync(Guid countryId);

        Task<IEnumerable<SelectListItem>> GetDDLCitiesAsync(Guid stateId);
    }
}
