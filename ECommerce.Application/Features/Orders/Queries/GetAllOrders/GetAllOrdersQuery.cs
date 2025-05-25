using System.Collections.Generic;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetAllOrders
{
    public record GetAllOrdersQuery : IRequest<List<Order>>;
} 