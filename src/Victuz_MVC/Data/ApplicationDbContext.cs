using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Victuz_MVC.Models;

namespace Victuz_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Activity> Activity { get; set; } = default!;
        public DbSet<ActivityCategory> ActivityCategory { get; set; } = default!;
        public DbSet<ActivityCategoryLine> ActivityCategoryLine { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategory { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        
        }


        // Vaste Data
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // Category data
            ProductCategory productCategory1 = new ProductCategory
            {
                Id = 1,
                Name = "T-Shirt",
            };

            ProductCategory productCategory2 = new ProductCategory
            {
                Id = 2,
                Name = "Rood",
            };

            ProductCategory productCategory3 = new ProductCategory
            {
                Id = 3,
                Name = "Zwart",
            };

            // Product data

            Product product1 = new Product
            {
                Id = 1,
                Name = "T-Shirt",
                Description = "Dit is een t-shirt",
                Price = 10.00M,
            };  

            builder.Entity<ProductCategory>().HasData(productCategory1);
            builder.Entity<ProductCategory>().HasData(productCategory2);
            builder.Entity<ProductCategory>().HasData(productCategory3);
            builder.Entity<Product>().HasData(product1);
        }
    }
}
