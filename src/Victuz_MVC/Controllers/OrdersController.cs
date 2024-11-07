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
            return View(await context.Order.Include(o => o.OrderProducts).ThenInclude(op => op.Product).Where(o => o.OrderProducts.Count > 0).ToListAsync());
        }

        // GET: Orders/Cart
        [Authorize]
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

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await context.Order
                .Include(o => o.Account)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {
                return NotFound();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddItemToCart([Bind("ProductId,Quantity")] AddCartItemViewModel addCartItem)
        {
            var account = await userManager.GetUserAsync(HttpContext.User);

            if (account is null)
            {
                return Unauthorized();
            }

            try
            {
                var order = await orderService.FindOrCreateOrder(account.Id);
                await orderService.AddOrderLine(order, addCartItem.ProductId, addCartItem.Quantity);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Er is iets mis gegaan met het toevoegen van item aan een order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SetProductQuantity([Bind("ProductId,Quantity")] AddCartItemViewModel addCartItem)
        {
            var account = await userManager.GetUserAsync(HttpContext.User);

            if (account is null)
            {
                return Unauthorized();
            }

            try
            {
                var order = await orderService.FindOrCreateOrder(account.Id);
                await orderService.SetProductQuantity(order, addCartItem.ProductId, addCartItem.Quantity);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Er is iets mis gegaan met het toevoegen van item aan een order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RemoveOrderLine([Bind("ProductId")] AddCartItemViewModel addCartItem)
        {
            var account = await userManager.GetUserAsync(HttpContext.User);

            if (account is null)
            {
                return Unauthorized();
            }

            try
            {
                var order = await orderService.FindOrCreateOrder(account.Id);
                await orderService.RemoveOrderLine(order, addCartItem.ProductId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Er is iets mis gegaan met het toevoegen van item aan een order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Success()
        {
            return View();
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
            catch (Exception e)
            {
                logger.LogError(e, "Er is iets mis gegaan met het bevestigen van een order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Optioneel: returnen naar een succes pagina
            return RedirectToAction(nameof(Success));
        }
    }
}
