using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechAssessment_C_.Data;
using TechAssessment_C_.Models;

namespace TechAssessment_C_.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ApplicationDBContext db, ILogger<ProductController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<Products> products = await _db.Products.ToListAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                throw;
            }
        }

        //////////////////////////SEARCH PRODUCT//////////////////////////

        //POST
        [HttpPost]
        public async Task<IActionResult> Search_Product(string searchInput)
        {
            try
            {
                if (string.IsNullOrEmpty(searchInput) || !int.TryParse(searchInput, out int id))
                {
                    TempData["error"] = "Invalid product ID format.";
                    return RedirectToAction("Index");
                }

                var product = await _db.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["error"] = "Product not found.";
                    return RedirectToAction("Index");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for a product.");
                TempData["error"] = "An error occurred while searching for a product.";
                return RedirectToAction("Index");
            }
        }

        //////////////////////////CREATE PRODUCT//////////////////////////

        //GET
        public IActionResult Create_Product()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_Product(Products product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ProductExists("Name", product.Name))
                    {
                        ModelState.AddModelError("name", "Product name already exists.");
                    }
                    else
                    {
                        _db.Products.Add(product);
                        await _db.SaveChangesAsync();
                        TempData["success"] = "Product added successfully.";
                        return RedirectToAction("Index");
                    }
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                TempData["error"] = "An error occurred while creating a product.";
                return RedirectToAction("Index");
            }
        }

        //////////////////////////EDIT PRODUCT//////////////////////////

        //GET
        [HttpGet]
        public async Task<IActionResult> Edit_Product(int id)
        {
            try
            {
                var product = await _db.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product details for editing.");
                TempData["error"] = "An error occurred while fetching product details for editing.";
                return RedirectToAction("Index");
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_Product(Products obj)
        {
            try
            {
                if (await _db.Products.AnyAsync(c => c.Id != obj.Id && c.Name == obj.Name))
                {
                    ModelState.AddModelError(nameof(obj.Name), "Product name already exists.");
                }

                if (ModelState.IsValid)
                {
                    var x = await _db.Products.FindAsync(obj.Id);
                    if (x == null)
                    {
                        return NotFound();
                    }

                    x.Name = obj.Name;
                    x.Description = obj.Description;
                    x.Price = obj.Price;

                    _db.Products.Update(x);
                    await _db.SaveChangesAsync();
                    TempData["info"] = "Product with ID: " + obj.Id + " was successfully updated.";
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing a product.");
                TempData["error"] = "An error occurred while editing a product.";
                return RedirectToAction("Index");
            }
        }


        //////////////////////////DELETE PRODUCT//////////////////////////

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete_Product(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                TempData["error"] = $"Product with ID: {id} not found.";
                return RedirectToAction(nameof(Index));
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            TempData["success"] = $"Product with ID: {id} was successfully deleted.";
            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(string propertyName, string propertyValue)
        {
            return _db.Products.Any(c => EF.Property<string>(c, propertyName) == propertyValue);
        }
    }
}
