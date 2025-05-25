using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.Exceptions;
using ECommerce.Contracts.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, CompleteOrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBalanceOrderService _balanceOrderService;

        public CompleteOrderCommandHandler(IOrderRepository orderRepository, IBalanceOrderService balanceOrderService)
        {
            _orderRepository = orderRepository;
            _balanceOrderService = balanceOrderService;
        }

        public async Task<CompleteOrderResponse> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                if (order == null)
                {
                    throw new OrderNotFoundException(request.OrderId);
                }

                var completeResponse = await _balanceOrderService.CompleteAsync(request.OrderId);
                if (!completeResponse.Success)
                {
                    throw new BalanceCompleteFailedException($"Failed to complete order. Message: {completeResponse.Data.Status}");
                }

                await _orderRepository.UpdateStatusAsync(request.OrderId, "completed");

                return new CompleteOrderResponse
                {
                    OrderId = request.OrderId,
                    Status = "completed"
                };
            }
            catch (Exception ex) when (ex is not (OrderNotFoundException or BalanceCompleteFailedException))
            {
                throw new BalanceCompleteFailedException("An error occurred while processing the complete request", ex);
            }
        }
    }
} 