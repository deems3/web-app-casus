using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Victuz_MVC.Data;
using Victuz_MVC.Models;
using Victuz_MVC.Services;
using Victuz_MVC.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Runtime.ExceptionServices;
using Victuz_MVC.Enums; // Nodig voor webhook notificatie

namespace Victuz_MVC.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly HttpClient _httpClient;
        private readonly PictureService _pictureService;

        public ActivitiesController(ApplicationDbContext context, UserManager<Account> userManager, PictureService pictureService, HttpClient httpClient)
        {
            _context = context;
            _userManager = userManager;
            _pictureService = pictureService;
            _httpClient = httpClient;
        }

        // GET: Activities
        // Index is visible for everybody. Only approved activities need to be displayed here.
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var isMember = user is not null ? await _userManager.IsInRoleAsync(user, "Member") : true;
            // Get all activities with corresponding hosts
            var activities = await _context.Activity
                .Include(a => a.Hosts)
                .Include(a => a.Picture)
                .Include(a => a.ActivityCategory)
                .Where(a => a.Status == Enums.ActivityStatus.Approved)
                .ToListAsync();
            ViewBag.IsMember = isMember;
            // ?? = null-coalesence
            // user?.Id ?? null means: if the user.Id is not null, use its value otherwise set to null
            ViewBag.UserId = user?.Id ?? null;

            ViewBag.IsBlacklisted = user?.Blacklisted ?? false;

            return View(activities);
        }

        // New iActionResult for a new view named StatusActivity. This view is for activities with processing status, only visibile for members and admins.
        public async Task<IActionResult> StatusActivity()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var isMember = user is not null ? await _userManager.IsInRoleAsync(user, "Member") : true;
            // Get all activities with corresponding hosts
            var activities = await _context.Activity
                .Include(a => a.Hosts)
                .Include(a => a.Picture)
                .Include(a => a.ActivityCategory)
                .Where(a => a.Status == Enums.ActivityStatus.Processing)
                .ToListAsync();
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
                .Include(a => a.Picture)
                .Include(a => a.ActivityCategory)
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
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategory, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.      
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Limit,DateTime,ActivityCategoryId,Location")] CreateActivityViewModel activityViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var activity = new Activity
                {
                    ActivityCategoryId = activityViewModel.ActivityCategoryId,
                    DateTime = activityViewModel.DateTime,
                    Description = activityViewModel.Description,
                    Limit = activityViewModel.Limit,
                    Name = activityViewModel.Name,
                    Status = Enums.ActivityStatus.Approved,
                    Location = activityViewModel.Location
                };

                if (file != null)
                {
                    var picture = await _pictureService.CreatePicture(file);
                    activity.Picture = picture;
                }

                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityViewModel);
        }

        // SUGGEST ACTIVITY FOR MEMBERS ONLY !! 
        [Authorize]
        public IActionResult Suggest()
        {
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategory, "Id", "Name");
            ViewData["Hosts"] = new MultiSelectList(_context.Accounts, "Id", "FirstName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suggest([Bind("Name,Description,Limit,DateTime,HostIds,ActivityCategoryId,Location")] CreateActivityViewModel activityViewModel, IFormFile? file)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid && activityViewModel.HostIds?.Count == 2)
            {
                var activity = new Activity
                {
                    ActivityCategoryId = activityViewModel.ActivityCategoryId,
                    Location = activityViewModel.Location,
                    DateTime = activityViewModel.DateTime,
                    Description = activityViewModel.Description,
                    Limit = activityViewModel.Limit,
                    Name = activityViewModel.Name,
                    Status = Enums.ActivityStatus.Processing
                };


                if (file != null)
                {
                    var picture = await _pictureService.CreatePicture(file);
                    activity.Picture = picture;
                }

                var hosts = new List<Account>()
                {
                    user
                };

                if (activityViewModel.HostIds is not null)
                {
                    foreach (var id in activityViewModel.HostIds)
                    {
                        var host = await _context.Accounts.FirstOrDefaultAsync(h => h.Id == id);
                        if (host is not null)
                        {
                            hosts.Add(host);
                        }
                    }
                }

                activity.Hosts.AddRange(hosts);
                _context.Add(activity);

                var url = "https://eoyk4vg91shywto.m.pipedream.net";

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                var categoryName = await _context.ActivityCategory
                    .Where(category => category.Id == activity.ActivityCategoryId)
                    .Select(category => category.Name)
                    .FirstOrDefaultAsync();

                var payload = JsonSerializer.Serialize(new { 
                    Data = activity,
                    Category = categoryName,
                    Hosts = activity.Hosts.Select(host => new { host.FirstName, host.Email })}, 
                    options);

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                await _httpClient.PostAsync(url, content);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategory, "Id", "Name");
            ViewData["Hosts"] = new MultiSelectList(_context.Accounts, "Id", "FirstName");
            return View("Suggest", activityViewModel);
        }


        [Authorize(Roles = "Admin,Member")]
        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is null)
            {
                return Unauthorized();
            }

            var isMember = await _userManager.IsInRoleAsync(user, "Member");

            // Fetch activiteits and their hosts
            var activity = await _context.Activity
                .Include(a => a.ActivityCategory)
                .Include(a => a.Picture)
                .Include(a => a.Hosts)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            if (isMember && !activity.Hosts.Any(h => h.Id == user.Id))
            {
                return Unauthorized();
            }

            // Check if user has member role, if so check if member is activity host, if not, dont show activity.
            if (await _userManager.IsInRoleAsync(user, "Member") && !activity.Hosts!.Any(h => h.Id == user.Id))
            {
                return Unauthorized();
            }

            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategory, "Id", "Name");

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Limit,DateTime,Status,ActivityCategoryId,Location")] Activity activity, IFormFile? file)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Picture? removedPicture = null;
                    // Fetch the existing entry so we can override the picture
                    var existingEntry = _context.Activity.AsNoTracking().Include(a => a.Picture).First(p => p.Id == activity.Id);
                    if (file != null)
                    {
                        
                        // Delete existing file from filesystem
                        if (existingEntry.Picture is not null)
                        {
                            _pictureService.DeletePicture(existingEntry.Picture.FilePath);
                            removedPicture = existingEntry.Picture;
                        }

                        var picture = await _pictureService.CreatePicture(file);
                        activity.Picture = picture;
                    }
                    else
                    {
                        if (existingEntry.Picture is not null)
                        {
                            activity.Picture = existingEntry.Picture;
                        }
                    }

                    activity.Hosts.AddRange(existingEntry.Hosts);


                    if (existingEntry != null)
                    {

                        if (activity.Status != existingEntry.Status)
                        {
                            var url = "https://eo6rv3rphu7vb23.m.pipedream.net";

                            var options = new JsonSerializerOptions
                            {
                                ReferenceHandler = ReferenceHandler.Preserve,
                                WriteIndented = true
                            };

                            var hostIds = activity.Hosts.Select(h => h.Id).ToList();

                            var accounts = await _context.Accounts
                                .Where(a => hostIds.Contains(a.Id))
                                .ToListAsync();

                            var payload = JsonSerializer.Serialize(new
                            {
                                Data = activity,
                                Status = activity.Status.ToString(),
                                Hosts = existingEntry.Hosts.Select(host => new { host.FirstName, host.Email })
                            }, options);

                            var content = new StringContent(payload, Encoding.UTF8, "application/json");
                            await _httpClient.PostAsync(url, content);
                        }

                        _context.Activity.Update(activity);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }

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
                .Include(a => a.ActivityCategory)
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
