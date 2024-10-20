using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz_MVC.Data;
using Victuz_MVC.Models;

namespace Victuz_MVC.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;

        public ActivitiesController(ApplicationDbContext context, UserManager<Account> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var isMember = user is not null ? await _userManager.IsInRoleAsync(user, "Member") : true;
            // Get all activities with corresponding hosts
            var activities = await _context.Activity.Include(a => a.Hosts).ToListAsync();
            ViewBag.IsMember = isMember;
            // ?? = null-coalesence
            // user?.Id ?? null means: if the user.Id is not null, use its value otherwise set to null
            ViewBag.UserId = user?.Id ?? null;

            return View(activities);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var isMember = user is not null ? await _userManager.IsInRoleAsync(user, "Member") : true;
            // Get all activities with corresponding hosts
            ViewBag.IsMember = isMember;
            // ?? = null-coalesence
            // user?.Id ?? null means: if the user.Id is not null, use its value otherwise set to null
            ViewBag.UserId = user?.Id ?? null;

            var activity = await _context.Activity
                .Include(a => a.Hosts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [Authorize(Roles = "Admin")]
        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.      
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Limit,DateTime,Status,ActivityCategoryLineId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        [Authorize(Roles = "Admin,Member")]
        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id) // TODO: ADD CHECK IF ROLE IS MEMBER IF ACTIVITY IS OWNED BY MEMBER
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch activiteits and their hosts
            var activity = await _context.Activity.Include(a => a.Hosts).FirstOrDefaultAsync(a => a.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            // Get logged in user
            var user = (await _userManager.GetUserAsync(HttpContext.User))!;

            // Check if user has member role, if so check if member is activity host, if not, dont show activity.
            if (await _userManager.IsInRoleAsync(user, "Member") && !activity.Hosts!.Any(h => h.Id == user.Id))
            {
                return Unauthorized();
            }

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Limit,DateTime,Status,ActivityCategoryLineId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        [Authorize(Roles = "Admin")]
        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [Authorize(Roles = "Admin")]
        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activity.FindAsync(id);
            if (activity != null)
            {
                _context.Activity.Remove(activity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }
    }
}
