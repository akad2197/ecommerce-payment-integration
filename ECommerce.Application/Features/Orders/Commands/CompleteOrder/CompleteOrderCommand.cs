using MediatR;

namespace ECommerce.Application.Features.Orders.Commands.CompleteOrder
{
    public record CompleteOrderCommand : IRequest<CompleteOrderResponse>
    {
        public string OrderId { get; init; }
    }
} 