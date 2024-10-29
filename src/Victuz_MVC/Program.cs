using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using Victuz_MVC.Data;
using Victuz_MVC.Models;
using Victuz_MVC.Services;

namespace Victuz_MVC
{
    public class Program
    {
        //Use async Task to run the program with a 'await' in identityroles
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<Account>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() 
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<PictureService>();

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                 name: "createEnrollment",
                pattern: "Enrollments/Create/{activityId?}",
                defaults: new { controller = "Enrollments", action = "Create" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Manager", "Member" };

                foreach (var role in roles)
                {
                    //Check if role already exists in the application, otherwise add new role.
                    if (!await RoleManager.RoleExistsAsync(role))
                        await RoleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<Account>>();

                var accounts = new Dictionary<string, string>
                {
                    { "demi@demi.nl", "Demi123!" },
                    { "mees@mees.nl", "Mees123!" },
                    { "aaron@aaron.nl", "Aaron123!" },
                    { "martijn@martijn.nl", "Martijn123!" },
                    { "weningmartijn@gmail.com", "Martijn123!" } // Graag deze niet gebruiken om mails te testen.
                };

                // email = key, password = value (Dictonary KeyValue pair)
                foreach (var (email, password) in accounts)
                {
                    if (await UserManager.FindByEmailAsync(email) == null)
                    {
                        // create the user
                        var user = new Account();
                        user.UserName = email;
                        user.Email = email;

                        // add user to database
                        await UserManager.CreateAsync(user, password);

                        // add to role
                        await UserManager.AddToRoleAsync(user, "Member");
                    }
                }

                string adminEmail = "admin@admin.com";
                string adminPassword = "Admin123!";

                if (await UserManager.FindByEmailAsync(adminPassword) == null)
                {
                    // create the admin
                    var user = new Account();
                    user.UserName = adminEmail;
                    user.Email = adminEmail;

                    // add admin to database
                    await UserManager.CreateAsync(user, adminPassword);

                    // add to role
                    await UserManager.AddToRoleAsync(user, "Admin");
                }

            }

            app.Run();
        }
    }
}
