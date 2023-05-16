using ECommer.DAL;
using ECommer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        private readonly IAzureBlobHelper _azureBlobHelper;
        private readonly IDropDownListHelper _dropDownListHelper;
        #endregion

        #region Builder
        public ProductsController(DataBaseContext context, IAzureBlobHelper azureBlobHelper, IDropDownListHelper dropDownListHelper)
        {
            _context = context;
            _azureBlobHelper = azureBlobHelper;
            _dropDownListHelper = dropDownListHelper;
        }
        #endregion

        #region Product actions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }
        #endregion
    }
}
