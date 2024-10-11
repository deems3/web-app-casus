using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Victuz_MVC.Models;

namespace Victuz_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Victuz_MVC.Models.Activity> Activity { get; set; } = default!;
        public DbSet<Victuz_MVC.Models.ActivityCategory> ActivityCategory { get; set; } = default!;
        public DbSet<Victuz_MVC.Models.Product> Products { get; set; } = default!;
        public DbSet<Victuz_MVC.Models.ProductCategory> ProductCategory { get; set; } = default!;
    }
}
