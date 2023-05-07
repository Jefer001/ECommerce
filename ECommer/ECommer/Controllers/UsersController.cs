using ECommer.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommer.Controllers
{
    public class UsersController : Controller
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public UsersController(DataBaseContext context)
        {
            _context = context;
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
        #endregion
    }
}
