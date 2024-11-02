using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Victuz_MVC.Data;
using Victuz_MVC.Models;

namespace Victuz_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountsController(ApplicationDbContext context) : Controller
    {
        // GET: UserController
        public async Task<ActionResult> Index()
        {
            return View(await context.Accounts.ToListAsync());
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            return View(await context.Accounts.FirstOrDefaultAsync(a => a.Id == id));
        }

        // POST: UserController/Edit/02c99dd1-c36c-40b3-aa23-92620ba6c2ba
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Blacklisted")] Account? account)
        {
            if (account is null)
            {
                return UnprocessableEntity();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(account);
                }

                var targetEntity = context.Accounts.First(a => a.Id == id);

                if (targetEntity.FirstName != account.FirstName)
                {
                    targetEntity.FirstName = account.FirstName;
                }

                if (targetEntity.LastName != account.LastName)
                {
                    targetEntity.LastName = account.LastName;
                }

                if (targetEntity.Blacklisted != account.Blacklisted)
                {
                    targetEntity.Blacklisted = account.Blacklisted;
                }

                context.Update(targetEntity);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(account);
            }
        }
    }
}
