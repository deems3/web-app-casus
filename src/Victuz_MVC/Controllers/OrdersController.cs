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
