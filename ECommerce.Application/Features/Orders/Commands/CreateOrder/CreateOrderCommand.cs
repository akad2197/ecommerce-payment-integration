using System.Collections.Generic;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public string OrderId { get; set; }
        public List<OrderProductDto> Products { get; set; }
    }

    public class OrderProductDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
} 