﻿using ECommer.DAL;
using ECommer.DAL.Entities;
using ECommer.Helpers;
using ECommer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECommer.Controllers
{
    public class HomeController : Controller
    {
        #region Constants
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;
        private readonly IUserHelpers _userHelper;
        //private readonly IOrderHelper _orderHelper;
        #endregion

        #region Builder
        public HomeController(ILogger<HomeController> logger, DataBaseContext context, IUserHelpers userHelpers/*, IOrderHelper orderHelper*/)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelpers;
            //_orderHelper = orderHelper;
        }
        #endregion

        #region Home actions
        public async Task<IActionResult> Index()
        {
            List<Product>? products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .OrderBy(p => p.Description)
                .ToListAsync();

            //Variables de Sesión
            ViewBag.UserFullName = GetUserFullName();

            HomeViewModel homeViewModel = new()
            {
                Products = products
            };

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                homeViewModel.Quantity = await _context.TemporalSales
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> AddProductInCart(Guid? productId)
        {
            if (productId == null) return NotFound();

            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            Product product = await _context.Products.FindAsync(productId);
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (user == null || product == null) return NotFound();

            // Busca una entrada existente en la tabla TemporalSale para este producto y usuario
            TemporalSale existingTemporalSale = await _context.TemporalSales
                .Where(t => t.Product.Id == productId && t.User.Id == user.Id)
                .FirstOrDefaultAsync();

            if (existingTemporalSale != null)
            {
                // Si existe una entrada, incrementa la cantidad
                existingTemporalSale.Quantity += 1;
                existingTemporalSale.ModifiedDate = DateTime.Now;
            }
            else
            {
                // Si no existe una entrada, crea una nueva
                TemporalSale temporalSale = new()
                {
                    CreatedDate = DateTime.Now,
                    Product = product,
                    Quantity = 1,
                    User = user
                };

                _context.TemporalSales.Add(temporalSale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsProduct(Guid? productId)
        {
            if (productId == null) return NotFound();

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            string categories = string.Empty;

            foreach (ProductCategory? category in product.ProductCategories)
                categories += $"{category.Category.Name}, ";

            categories = categories.Substring(0, categories.Length - 2);

            DetailsProductToCartViewModel detailsProductToCartViewModel = new()
            {
                Categories = categories,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages,
                Quantity = 1,
                Stock = product.Stock,
            };

            return View(detailsProductToCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsProduct(DetailsProductToCartViewModel detailsProductToCartViewModel)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            Product product = await _context.Products.FindAsync(detailsProductToCartViewModel.Id);
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (product == null || user == null) return NotFound();

            // Busca una entrada existente en la tabla TemporalSale para este producto y usuario
            TemporalSale existingTemporalSale = await _context.TemporalSales
                .Where(t => t.Product.Id == detailsProductToCartViewModel.Id && t.User.Id == user.Id)
                .FirstOrDefaultAsync();

            if (existingTemporalSale != null)
            {
                // Si existe una entrada, incrementa la cantidad
                existingTemporalSale.Quantity += detailsProductToCartViewModel.Quantity;
                existingTemporalSale.ModifiedDate = DateTime.Now;
            }
            else
            {
                // Si no existe una entrada, crea una nueva
                TemporalSale temporalSale = new()
                {
                    Product = product,
                    Quantity = 1,
                    User = user,
                    Remarks = detailsProductToCartViewModel.Remarks,
                };

                _context.TemporalSales.Add(temporalSale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize] //Etiqueta para que solo usuarios logueados puedan acceder a este método.
        public async Task<IActionResult> ShowCartAndConfirm()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound();

            List<TemporalSale>? temporalSales = await _context.TemporalSales
                .Include(ts => ts.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            ShowCartViewModel showCartViewModel = new()
            {
                User = user,
                TemporalSales = temporalSales
            };

            return View(showCartViewModel);
        }
        #endregion

        #region Private methods
        private string GetUserFullName()
        {
            return _context.Users
                .Where(u => u.Email == User.Identity.Name)
                .Select(u => u.FullName.ToUpper())
                .FirstOrDefault();
        }
        #endregion
    }
}