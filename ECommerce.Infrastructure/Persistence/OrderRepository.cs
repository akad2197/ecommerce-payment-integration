using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await Task.FromResult(_orders.Find(x => x.OrderId == id));
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await Task.FromResult(_orders);
        }

        public async Task SaveAsync(Order order)
        {
            _orders.Add(order);
            await Task.CompletedTask;
        }

        public async Task UpdateStatusAsync(string orderId, string status)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
            }
            await Task.CompletedTask;
        }
    }
} 