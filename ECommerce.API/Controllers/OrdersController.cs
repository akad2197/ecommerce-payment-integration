using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Application.Features.Orders.Commands.CompleteOrder;
using ECommerce.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.Application.Features.Orders.Queries.GetAllOrders;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Order>), 200)]
        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllAsync();
        }

        /// <summary>
        /// Gets an order by its ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="command">Order creation details</param>
        /// <returns>Created order information</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Completes an order
        /// </summary>
        /// <param name="command">Complete order request</param>
        /// <returns>Completion status</returns>
        [HttpPost("complete")]
        [ProducesResponseType(typeof(CompleteOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CompleteOrder(CompleteOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
} 