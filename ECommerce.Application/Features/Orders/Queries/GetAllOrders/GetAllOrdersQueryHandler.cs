using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler(IOrderRepository orderRepository) 
        : IRequestHandler<GetAllOrdersQuery, List<Order>>
    {
        public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await orderRepository.GetAllAsync();
        }
    }
} 