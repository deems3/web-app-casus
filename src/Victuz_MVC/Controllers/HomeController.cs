using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Victuz_MVC.Data;
using Victuz_MVC.Models;

namespace Victuz_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public ActionResult Index()
        {
            var upcomingActivity = _context.Activity
                .Where(a => a.DateTime > DateTime.Now)
                .OrderBy(a => a.DateTime)
                .FirstOrDefault();

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
