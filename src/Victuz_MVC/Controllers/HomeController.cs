using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Victuz_MVC.Data;
using Victuz_MVC.Models;

namespace Victuz_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<Account> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var upcomingActivity = _context.Activity
                .Include(a => a.Picture)
                .Where(a => a.DateTime > DateTime.Now)
                .OrderBy(a => a.DateTime)
            .FirstOrDefault();

            var user = HttpContext.User;

            if (user.Identity is null || !user.Identity.IsAuthenticated)
            {
                ViewBag.IsBlacklisted = true;
            } 
            else
            {
                var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
                if (loggedInUser == null)
                {
                    ViewBag.IsBlacklisted = true;
                    return View(upcomingActivity);
                }
                ViewBag.IsBlacklisted = loggedInUser.Blacklisted;
            }
            return View(upcomingActivity);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
