using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITI_Graduation_Project.Data;
using ITI_Graduation_Project.Models;

namespace ITI_Graduation_Project.Controllers
{
    public class CustomersController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Customers
        public async Task<IActionResult> Index(string? search, string sortField = "Name", string sortOrder = "asc")
        {
            ViewBag.CurrentSearch = search;
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            var customers = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                customers = customers.Where(e => e.FullName.Contains(search) || e.Email.Contains(search) || (e.Phone.HasValue && EF.Functions.Like(e.Phone.ToString(), "%" + search + "%")) || e.Address.Contains(search));
            }
            switch (sortField)
            {
                case "Email":
                    customers = sortOrder == "asc"
                        ? customers.OrderBy(p => p.Email)
                        : customers.OrderByDescending(p => p.Email);
                    break;
                case "Phone":
                    customers = sortOrder == "asc"
                        ? customers.OrderBy(p => p.Phone)
                        : customers.OrderByDescending(p => p.Phone);
                    break;
                case "Address":
                    customers = sortOrder == "asc"
                        ? customers.OrderBy(p => p.Address)
                        : customers.OrderByDescending(p => p.Address);
                    break;
                case "TimeAdded":
                    customers = sortOrder == "asc"
                        ? customers.OrderBy(p => p.Id)
                        : customers.OrderByDescending(p => p.Id);
                    break;
                default:
                    customers = sortOrder == "asc"
                        ? customers.OrderBy(p => p.FullName)
                        : customers.OrderByDescending(p => p.FullName);
                    break;
            }
            return View(await customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(e => e.Products)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Phone,Address,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Phone,Address,Password")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Customers.FindAsync(id);
                    if (existing == null) return NotFound();
                    existing.FullName = customer.FullName;
                    existing.Email = customer.Email;
                    existing.Phone = customer.Phone;
                    existing.Address = customer.Address;
                    if (!string.IsNullOrWhiteSpace(customer.Password))
                    {
                        existing.Password = customer.Password;
                    }
                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
