using Microsoft.EntityFrameworkCore;
using Victuz_MVC.Data;
using Victuz_MVC.Enums;
using Victuz_MVC.Models;

namespace Victuz_MVC.Services
{
    public class OrderService(ApplicationDbContext context)
    {
        public async Task<Order> FindOrCreateOrder(string accountId)
        {
            // Fetch an order for the current user that's open and not completed
            var order = await context.Order
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(
                    o => o.AccountId == accountId
                    && o.Status == OrderStatus.Initialized
                    && o.CompletedAt == null
            );

            // If no open order exists for the user, create a new one and save it to the database
            if (order == null)
            {
                order = new Order
                {
                    AccountId = accountId,
                };

                context.Order.Add(order);
                await context.SaveChangesAsync();
            }

            return order;
        }

        public async Task AddOrderLine(Order order, int productId, int amount = 1)
        {
            // Check if a product already exists for the given order
            // TODO: consider querying the context.OrderProduct directly, to prevent foreign key constraint failures (and is probably more performant)
            var existingProduct = order.OrderProducts.FirstOrDefault(p => p.Product.Id == productId);

            // Product does not exist, create new OrderLine/OrderProduct and add the product to it
            if (existingProduct is null)
            {
                order.OrderProducts.Add(
                    new OrderProduct
                    {
                        ProductId = productId,
                        ProductAmount = amount,
                        Order = order // Could be redundant or cause error because of foreign key constraint, if it does, remove this line
                    }
                );
            }
            else
            {
                // Product exists, increment the amount by given amount (default 1)
                existingProduct.ProductAmount += amount;
            }

            context.Order.Update(order);
            await context.SaveChangesAsync();
        }

        public async Task RemoveOrderLine(Order order, Product product, int amount = 1)
        {
            // Check if product exists for order
            var existingProduct = order.OrderProducts.FirstOrDefault(p => p.Product.Id == product.Id);

            // Product already deleted (edge case/race condition)
            if (existingProduct is null)
            {
                return;
            }
            else
            {
                // Product does not exit, decrease the amount
                existingProduct.ProductAmount -= amount;

                // If amount gets to or below 0, remove the item from the order
                if (existingProduct.ProductAmount <= 0)
                {
                    order.OrderProducts.Remove(existingProduct);
                }
            }

            context.Order.Update(order);
            await context.SaveChangesAsync();
        }

        public async Task ConfirmOrder(int orderId)
        {
            var order = await context.Order.FirstAsync(o => o.Id == orderId);
            order.CompletedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Processing;

            context.Order.Update(order);
            await context.SaveChangesAsync();
        }
    }

}
