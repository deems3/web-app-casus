﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategory { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ProductCategory productCategory1 = new ProductCategory
            {
                Id = 1,
                Name = "Testproductcategorie 1",
            };

            Product product1 = new Product
            {
                Id = 1,
                Name = "Testproduct 1",
                Description = "Het eerste testproduct",
                Price = 19.99m,
                CategoryId = 1
            };

            builder.Entity<ProductCategory>().HasData(productCategory1);
            builder.Entity<Product>().HasData(product1);
        }
    }
}
