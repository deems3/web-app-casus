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
                .Include(p => p.ProductCategoryLines)
                .ThenInclude(pcl => pcl.ProductCategory)
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
                .Include(p => p.ProductCategoryLines)
                .ThenInclude(pcl => pcl.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new MultiSelectList(_context.ProductCategory, "Id", "Name");
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
                    ProductCategoryLines = []
                };

                if (file != null)
                {
                    var picture = await _pictureService.CreatePicture(file);
                    product.Picture = picture;
                }

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

            var product = await _context.Products
                .Include(p => p.ProductCategoryLines)
                .ThenInclude(pcl => pcl.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var selectedProductCategories = product.ProductCategoryLines.Select(p => p.ProductCategory).ToList();

            ViewBag.Categories = new MultiSelectList(_context.ProductCategory, "Id", "Name", selectedValues: selectedProductCategories.Select(spc => spc!.Id));

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Product product, List<int> selectedCategoryIds, IFormFile? file)
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
                    var existingEntry = await _context.Products
                        .AsNoTracking()
                        .Include(p => p.Picture)
                        .FirstOrDefaultAsync(p => p.Id == product.Id);

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

                    var existingCategoryLines = await _context.ProductCategoryLine
                        .Where(pcl => pcl.ProductId == product.Id)
                        .Include(pcl => pcl.ProductCategory)
                        .ToListAsync();
                    var removedCategories = existingCategoryLines
                            .Where(p => !selectedCategoryIds.Contains(p.ProductCategory!.Id))
                            .ToList();
                    var existingCategories = existingCategoryLines
                            .Where(p => selectedCategoryIds.Contains(p.ProductCategory!.Id))
                            .ToList();

                    foreach (var cat in removedCategories)
                    {
                        _context.ProductCategoryLine.Remove(cat);
                    }

                    List<ProductCategoryLine> categoryLinesToAdd = [];

                    foreach (var category in existingCategories)
                    {
                        if (!selectedCategoryIds.Contains(category.ProductCategory!.Id))
                        {
                            continue;
                        }
                        selectedCategoryIds.Remove(category.ProductCategory.Id);
                    }

                    foreach (var idToCreate in selectedCategoryIds)
                    {
                        _context.ProductCategoryLine.Add(
                            new ProductCategoryLine
                            {
                                Product = product,
                                ProductCategoryId = idToCreate,
                            }
                        );
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
