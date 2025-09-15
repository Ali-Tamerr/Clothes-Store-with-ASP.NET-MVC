using ITI_Graduation_Project.Data;
using ITI_Graduation_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ITI_Graduation_Project.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Products owned: total number of products in the store
            var productsOwned = _context.Products.Count();
            // Number of purchases: total number of purchase records
            var totalPurchases = _context.Purchases.Count();

            // Recent purchases: last 5 purchases with related product and customer
            var recentPurchases = _context.Purchases
                .Include(p => p.Product)
                .Include(p => p.Customer)
                .OrderByDescending(p => p.PurchaseDate)
                .Take(5)
                .ToList();

            ViewBag.ProductsOwned = productsOwned;
            ViewBag.TotalPurchases = totalPurchases;
            ViewBag.RecentPurchases = recentPurchases;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ClientSide()
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.Customers).ToList();
            Customer currentCustomer = null;
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId != null)
            {
                currentCustomer = _context.Customers.Include(c => c.Products).FirstOrDefault(c => c.Id == customerId);
            }
            ViewBag.CurrentCustomer = currentCustomer;
            return View(products);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        public async Task<IActionResult> ClientProductDetailsAsync(int? id)
        {
            Product product = await _context.Products.Include(p => p.Category)
               .Include(p => p.Customers)
               .FirstOrDefaultAsync(m => m.Id == id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Buy(int id)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (customerId == null)
            {
                TempData["BuyWarning"] = "You must log in to buy a product.";
                return RedirectToAction("Login", "Home");
            }

            var product = _context.Products.Include(p => p.Customers).FirstOrDefault(p => p.Id == id);
            var customer = _context.Customers.Include(c => c.Products).FirstOrDefault(c => c.Id == customerId);

            if (product != null && customer != null && !product.Customers.Any(c => c.Id == customer.Id))
            {
                product.Customers.Add(customer);
                _context.SaveChanges();
                // Create a purchase record
                var purchase = new Purchase { ProductId = product.Id, CustomerId = customer.Id, PurchaseDate = DateTime.Now };
                _context.Purchases.Add(purchase);
                _context.SaveChanges();
            }

            return RedirectToAction("ClientSide");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
