using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery : IRequest<Order?>
    {
        public string OrderId { get; init; }
    }
} 