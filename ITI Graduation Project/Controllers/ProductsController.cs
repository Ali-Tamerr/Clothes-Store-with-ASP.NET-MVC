using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITI_Graduation_Project.Data;
using ITI_Graduation_Project.Models;
using Microsoft.AspNetCore.Hosting;

namespace ITI_Graduation_Project.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        private readonly IHostEnvironment _env;

        public ProductsController(IHostEnvironment env) => _env = env;

        // GET: Products
        public async Task<IActionResult> Index(string? search, string sortField = "Name", string sortOrder = "asc")
        {
            ViewBag.CurrentSearch = search;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            var products = _context.Products.Include(p => p.Category).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(e => e.Name.Contains(search) || e.Description.Contains(search));
            }
            switch (sortField)
            {
                case "Category":
                    products = sortOrder == "asc"
                        ? products.OrderBy(p => p.Category.Name)
                        : products.OrderByDescending(p => p.Category.Name);
                    break;
                case "ForAge":
                    products = sortOrder == "asc"
                        ? products.OrderBy(p => p.ForAge)
                        : products.OrderByDescending(p => p.ForAge);
                    break;
                case "TimeAdded":
                    products = sortOrder == "asc"
                        ? products.OrderBy(p => p.Id)
                        : products.OrderByDescending(p => p.Id);
                    break;
                default:
                    products = sortOrder == "asc"
                        ? products.OrderBy(p => p.Name)
                        : products.OrderByDescending(p => p.Name);
                    break;
            }
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            Product product = await _context.Products.Include(p => p.Category).Include(p => p.Customers).FirstOrDefaultAsync(m => m.Id == id);

            return View(product);
        }

        // POST: Products/Buy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id)
        {
            Product product = await _context.Products.Include(p => p.Customers).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null) return RedirectToAction("Login", "Home");

            var customer = await _context.Customers.FindAsync(customerId.Value);
            if (customer == null) return RedirectToAction("Login", "Home");
            if (!product.Customers.Any(c => c.Id == customer.Id))
            {
                product.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id });
        }
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.CategoriesSelectList = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            if (ModelState.IsValid)
            {

                var env = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(env.WebRootPath ?? "wwwroot", "images", "products");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(product.ImageFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }
                    product.ImagePath = $"/images/products/{uniqueFileName}";
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoriesSelectList = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Product product = await _context.Products.FindAsync(id);

            ViewBag.CategoriesSelectList = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {

            if (ModelState.IsValid)
            {

                var existing = await _context.Products.FindAsync(id);
                if (existing == null) return NotFound();
                existing.Name = product.Name; existing.Description = product.Description; existing.CategoryId = product.CategoryId; existing.ForAge = product.ForAge;
                var env = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(env.WebRootPath ?? "wwwroot", "images", "products");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(product.ImageFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }
                    if (!string.IsNullOrEmpty(existing.ImagePath))
                    {
                        try
                        {
                            var oldRelative = existing.ImagePath.TrimStart('/');
                            var oldPhysical = Path.Combine(env.WebRootPath ?? "wwwroot", oldRelative.Replace('/', Path.DirectorySeparatorChar));
                            if (System.IO.File.Exists(oldPhysical))
                                System.IO.File.Delete(oldPhysical);
                        }
                        catch { }
                    }
                    existing.ImagePath = $"/images/products/{uniqueFileName}";
                }
                try
                {
                    _context.Update(existing);
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
            ViewBag.CategoriesSelectList = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Product product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            Product product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                var env = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    try
                    {
                        var oldRelative = product.ImagePath.TrimStart('/');
                        var oldPhysical = Path.Combine(env.WebRootPath ?? "wwwroot", oldRelative.Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPhysical))
                            System.IO.File.Delete(oldPhysical);
                    }
                    catch { }
                }
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
