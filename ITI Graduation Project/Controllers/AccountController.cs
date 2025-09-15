using ITI_Graduation_Project.Data;
using ITI_Graduation_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_Graduation_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null || customer.Password != password)
            {
                ModelState.AddModelError(string.Empty, "Wrong email or password.");
                return View();
            }

            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            return RedirectToAction("ClientSide", "Home");
        }

        // GET: /Account/Signup
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = await _context.Customers.AnyAsync(c => c.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError(nameof(model.Email), "Email already in use.");
                return View(model);
            }

            _context.Customers.Add(model);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetInt32("CustomerId", model.Id);
            return RedirectToAction("ClientSide", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("ClientSide", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutAjax()
        {
            HttpContext.Session.Remove("CustomerId");
            return NoContent();
        }
    }
}
