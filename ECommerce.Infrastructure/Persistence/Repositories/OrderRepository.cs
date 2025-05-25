using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepository(OrderDbContext dbContext) : IOrderRepository
    {
        public async Task SaveAsync(Order order)
        {
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(string orderId)
        {
            return await dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await dbContext.Orders
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(string orderId, string newStatus)
        {
            var order = await dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null)
            {
                order.Status = newStatus;
                await dbContext.SaveChangesAsync();
            }
        }
    }
} 