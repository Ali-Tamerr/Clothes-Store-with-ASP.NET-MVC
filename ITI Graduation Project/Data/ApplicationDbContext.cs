using ITI_Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI_Graduation_Project.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=THISISALI;Database=ClothesShop;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
