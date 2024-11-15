﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz_MVC.Data;
using Victuz_MVC.Models;
using Victuz_MVC.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization; // Nodig voor webhook notificatie

namespace Victuz_MVC.Controllers
{
    [Authorize]
    public class EnrollmentsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;

        public EnrollmentsController(ApplicationDbContext context, UserManager<Account> userManager, HttpClient httpClient)
        {
            _context = context;
            _userManager = userManager;
            _httpClient = httpClient;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User))!;

            var applicationDbContext = _context.Enrollments
                .Include(e => e.Account)
                .Where(e => e.AccountId == user.Id)
                .Include(e => e.Activity);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Account)
                .Include(e => e.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public async Task<IActionResult> Create(int? activityId = null)
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User))!;

            var activities = new List<Activity>();

            ViewData["IsReadOnly"] = false;

            if (activityId is not null)
            {
                var activity = await _context.Activity.FirstOrDefaultAsync(a => a.Id == activityId);
                if (activity is null)
                {
                    return NotFound();
                }

                activities = [activity];

                ViewData["ActivityName"] = activities.FirstOrDefault()?.Name;
                ViewData["IsReadOnly"] = true;
            }
            else
            {
                activities = await _context.Activity
                .Include(a => a.Enrollments)
                .ThenInclude(e => e.Account)
                .Where(a => a.Enrollments.Count < a.Limit)
                .Where(a => !a.Enrollments.Any(e => e.AccountId == user.Id))
                .ToListAsync();
            }

            ViewData["ActivityId"] = new SelectList(activities, "Id", "Name");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityId")] CreateEnrollmentViewModel enrollment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user is null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var activity = await _context.Activity.Include(a => a.Enrollments).FirstOrDefaultAsync(a => a.Id == enrollment.ActivityId);
                if (activity is null)
                {
                    return NotFound();
                }

                if (activity.Enrollments.Any(e => e.AccountId == user.Id))
                {
                    ViewData["IsReadOnly"] = true;
                    ModelState.AddModelError("ActivityId", "Inschrijving niet mogelijk, je bent al ingeschreven");
                    return View(enrollment);
                }

                if ((activity.Enrollments.Count + 1) > activity.Limit)
                {
                    ViewData["IsReadOnly"] = true;
                    ModelState.AddModelError("ActivityId", "Inschrijving niet mogelijk, limiet bereikt");
                    return View(enrollment);
                }

                var entity = new Enrollment
                {
                    AccountId = user.Id,
                    ActivityId = enrollment.ActivityId,
                    EnrolledAt = DateTime.UtcNow,
                };

                    var url = "https://eobgobpreaytb0k.m.pipedream.net";
                    var payload = JsonSerializer.Serialize(new { Data = user, enrollment, activity });
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");

                    await _httpClient.PostAsync(url, content);


                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Name", enrollment.ActivityId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Account)
                .Include(e => e.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments
                            .Include(e => e.Account)
                            .Include(e => e.Activity)
                            .FirstOrDefaultAsync(m => m.Id == id);

            if (enrollment != null)
            {
                var url = "https://eomw1k0wa3oaj8t.m.pipedream.net";

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };
                var payload = JsonSerializer.Serialize(new { Data = enrollment }, options);

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                await _httpClient.PostAsync(url, content);

                _context.Enrollments.Remove(enrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.Id == id);
        }
    }
}
