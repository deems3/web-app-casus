using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz_MVC.Data;
using Victuz_MVC.Models;
using Victuz_MVC.Services;
using Victuz_MVC.ViewModels;

namespace Victuz_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PictureService _pictureService;

        public ProductsController(ApplicationDbContext context, PictureService pictureService)
        {
            _context = context;
            _pictureService = pictureService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {

            var products = await _context.Products
                .Include(p => p.Picture)
                .Include(c => c.ProductCategoryLines)
                    .ThenInclude(c => c.ProductCategory)
                .ToListAsync();

            return View(products);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Picture)
                .Include(p => p.ProductCategoryLines) // Voeg dit toe om categorieën te laden
                    .ThenInclude(c => c.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [Authorize(Roles = "Admin")]
        //GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name");
            return View();
        }



        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,SelectedCategoryIds")] CreateProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    ProductCategoryLines = new List<ProductCategoryLine>()
                };

                if (file != null)
                {
                    var picture = await _pictureService.CreatePicture(file);
                    product.Picture = picture;
                }

                // Add selected categories to the product
                foreach (var categoryId in productViewModel.SelectedCategoryIds)
                {
                    var categoryLine = new ProductCategoryLine
                    {
                        Product = product,
                        ProductCategoryId = categoryId
                    };
                    product.ProductCategoryLines.Add(categoryLine);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name");
            return View(productViewModel);
        }





        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Picture)
                .ToListAsync();
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Product product, IFormFile? file)
        {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                Picture? removedPicture = null;

                // Fetch the existing entry so we can override the picture
                var existingEntry = _context.Products.AsNoTracking().Include(a => a.Picture).FirstOrDefault(p => p.Id == product.Id);
                if (existingEntry == null)
                {
                    return NotFound(); // Return NotFound if the product does not exist
                }

                if (file != null)
                {
                    // Delete existing file from filesystem
                    if (existingEntry.Picture is not null)
                    {
                        _pictureService.DeletePicture(existingEntry.Picture.FilePath);
                        removedPicture = existingEntry.Picture;
                    }

                    var picture = await _pictureService.CreatePicture(file);
                    product.Picture = picture;
                }
                else
                {
                    if (existingEntry.Picture is not null)
                    {
                        product.Picture = existingEntry.Picture;
                    }
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        return RedirectToAction(nameof(Index));
    }
    return View(product);
}
        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
