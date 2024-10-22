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
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategory { get; set; } = default!;
        public DbSet<Picture> Picture { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        
        }


        // Vaste Data
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Activity>()
                .HasMany(e => e.Hosts)
                .WithMany(e => e.Activities);

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

            builder.Entity<ProductCategory>().HasData(productCategory1);
            builder.Entity<ProductCategory>().HasData(productCategory2);
            builder.Entity<ProductCategory>().HasData(productCategory3);

            // Producten

            Product product1 = new Product
            {
                Id = 1,
                Name = "T-Shirt",
                Description = "Dit is een t-shirt",
                Price = 10.00M,
            };

            Product product2 = new Product
            {
                Id = 2,
                Name = "Hoodie",
                Description = "Dit is een Hoodie",
                Price = 15.00M,
            };

            Product product3 = new Product
            {
                Id = 3,
                Name = "Pet",
                Description = "Dit is een petje",
                Price = 50.00M,
            };


            builder.Entity<Product>().HasData(product1);
            builder.Entity<Product>().HasData(product2);
            builder.Entity<Product>().HasData(product3);

            // Categories
            var cat1 = new ActivityCategory
            {
                Id = 1,
                Name = "Sport"
            };

            var cat2 = new ActivityCategory
            {
                Id = 2,
                Name = "Programming"
            };

            var cat3 = new ActivityCategory
            {
                Id = 3,
                Name = "AI"
            };

            builder.Entity<ActivityCategory>().HasData(cat1);
            builder.Entity<ActivityCategory>().HasData(cat2);
            builder.Entity<ActivityCategory>().HasData(cat3);

            // Activities
            var act1 = new Activity
            {
                Id = 1,
                Name = "Voetbal toernooi",
                Description = "Versla je medestudenten",
                Limit = 30,
                DateTime = DateTime.Parse("2024-11-23"),
                Status = Enums.ActivityStatus.Approved,
                ActivityCategoryId = 1
            };

            var act2 = new Activity
            {
                Id = 2,
                Name = "BattleBots",
                Description = "Bouw & programmeer je eigen BattleBot en versla je medestudenten",
                Limit = 30,
                DateTime = DateTime.Parse("2024-11-25"),
                Status = Enums.ActivityStatus.Approved,
                ActivityCategoryId = 2
            };

            var act3 = new Activity
            {
                Id = 3,
                Name = "Creatief met AI",
                Description = "Leer een eigen AI applicatie te bouwen",
                Limit = 13,
                DateTime = DateTime.Parse("2024-12-10"),
                Status = Enums.ActivityStatus.Approved,
                ActivityCategoryId = 3
            };

            builder.Entity<Activity>().HasData(act1);
            builder.Entity<Activity>().HasData(act2);
            builder.Entity<Activity>().HasData(act3);
        }
    }
}
