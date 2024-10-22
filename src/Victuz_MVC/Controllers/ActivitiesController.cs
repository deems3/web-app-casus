﻿using System;
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

namespace Victuz_MVC.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly PictureService _pictureService;

        public ActivitiesController(ApplicationDbContext context, UserManager<Account> userManager, PictureService pictureService)
        {
            _context = context;
            _userManager = userManager;
            _pictureService = pictureService;
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
        public async Task<IActionResult> Create([Bind("Name,Description,Limit,DateTime,ActivityCategoryId")] CreateActivityViewModel activityViewModel, IFormFile? file)
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
                    Status = Enums.ActivityStatus.Approved
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
            ViewData["Hosts"] = new MultiSelectList(_context.Accounts, "Id", "Email");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suggest([Bind("Name,Description,Limit,DateTime,HostIds,ActivityCategoryId")] CreateActivityViewModel activityViewModel)
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
                    Status = Enums.ActivityStatus.Processing
                };
                var hosts = new List<Account>();
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
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Suggest", activityViewModel);
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
            var activity = await _context.Activity
                .Include(a => a.ActivityCategory)
                .Include(a => a.Picture)
                .Include(a => a.Hosts)
                .FirstOrDefaultAsync(a => a.Id == id);
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

            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategory, "Id", "Name");

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Limit,DateTime,Status,ActivityCategoryId")] Activity activity, IFormFile? file)
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
                    var existingEntry = _context.Activity.Include(a => a.Picture).First(p => p.Id == activity.Id);
                    if (file != null)
                    {
                        
                        // Delete existing file from filesystem
                        if (existingEntry.Picture is not null)
                        {
                            _pictureService.DeletePicture(existingEntry.Picture.FilePath);
                            removedPicture = existingEntry.Picture;
                        }

                        var picture = await _pictureService.CreatePicture(file);
                        existingEntry.Picture.FilePath = picture.FilePath;
                        existingEntry.Picture.FileName = picture.FileName;
                    }

                    _context.Update(existingEntry);
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
