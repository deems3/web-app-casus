using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Victuz_MVC.Data;
using Victuz_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Victuz_MVC.ViewModels;
using Victuz_MVC.Services;
using Microsoft.AspNetCore.Identity;

namespace Victuz_MVC.Controllers
{
    public class OrdersController(
        ILogger<OrdersController> logger,
        ApplicationDbContext context,
        UserManager<Account> userManager,
        OrderService orderService
    ) : Controller
    {
        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await context.Order.Include(o => o.OrderProducts).ThenInclude(op => op.Product).ToListAsync());
        }

        // GET: Orders/Cart
        public async Task<IActionResult> Cart()
        {
            var account = await userManager.GetUserAsync(HttpContext.User);

            if (account is null)
            {
                return Unauthorized();
            }

            var order = await orderService.FindOrCreateOrder(account.Id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalPrice,AccountId")] Order order)
        {
            if (ModelState.IsValid)
            {
                context.Add(order);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalPrice,AccountId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(order);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await context.Order.FindAsync(id);
            if (order != null)
            {
                context.Order.Remove(order);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return context.Order.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddItemToCart([Bind("ProductId")] AddCartItemViewModel addCartItem)
        {
            var account = await userManager.GetUserAsync(HttpContext.User);

            if (account is null)
            {
                return Unauthorized();
            }

            try
            {
                var order = await orderService.FindOrCreateOrder(account.Id);
                await orderService.AddOrderLine(order, addCartItem.ProductId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Er is iets mis gegaan met het toevoegen van item aan een order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // TODO: consider returning the order to the user? OR redirect user to the cart
            return RedirectToAction(nameof(Cart));
            //return Ok();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ConfirmOrder([Bind("OrderId")] int orderId)
        {
            try
            {
                await orderService.ConfirmOrder(orderId);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Er is iets mis gegaan met het bevestigen van een order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Optioneel: returnen naar een succes pagina
            return RedirectToAction(nameof(Cart));
        }
    }
}
