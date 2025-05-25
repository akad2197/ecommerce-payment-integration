using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler(IOrderRepository orderRepository) 
        : IRequestHandler<GetOrderByIdQuery, Order?>
    {
        public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await orderRepository.GetByIdAsync(request.OrderId);
        }
    }
} 