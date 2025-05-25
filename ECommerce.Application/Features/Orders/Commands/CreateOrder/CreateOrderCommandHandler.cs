using ECommerce.Contracts.Exceptions;
using ECommerce.Contracts.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IBalanceOrderService balanceOrderService,
        IBalanceProductService balanceProductService) 
        : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {                
                var availableProducts = await balanceProductService.GetProductsAsync();
                
                foreach (var product in request.Products)
                {
                    var availableProduct = availableProducts.FirstOrDefault(p => p.Id == product.ProductId);
                    
                    if (availableProduct == null)
                    {
                        throw new ProductNotFoundException(product.ProductId);
                    }
                    
                    if (availableProduct.Stock < product.Quantity)
                    {
                        throw new InsufficientStockException(product.ProductId, availableProduct.Stock, product.Quantity);
                    }
                }

                var totalAmount = request.Products.Sum(p => 
                    availableProducts.First(ap => ap.Id == p.ProductId).Price * p.Quantity);

                var balance = await balanceOrderService.GetBalanceAsync();
                if (!balance.Success)
                {
                    throw new BalancePreorderFailedException("Failed to get balance information");
                }

                if (balance.Data.AvailableBalance < totalAmount)
                {
                    throw new InsufficientBalanceException(balance.Data.AvailableBalance, totalAmount);
                }

                var preorderRequest = new PreorderRequestDto
                {
                    OrderId = request.OrderId,
                    Amount = totalAmount,
                    Currency = balance.Data.Currency
                };

                var preorderResponse = await balanceOrderService.PreorderAsync(preorderRequest);
                Console.WriteLine("PreorderResponse: " + preorderResponse);
                if (!preorderResponse.Success)
                {
                    throw new BalancePreorderFailedException($"Balance preorder failed. Message: {preorderResponse.Data.Status}");
                }

                var order = new Order
                {
                    OrderId = request.OrderId,
                    Amount = totalAmount,
                    Currency = balance.Data.Currency,
                    ProductIds = request.Products.Select(p => p.ProductId).ToList(),
                    Status = "pending",
                    CreatedAt = DateTime.UtcNow
                };

                await _orderRepository.SaveAsync(order);

                return new CreateOrderResponse
                {
                    OrderId = request.OrderId,
                    Status = "success"
                };
            }
            catch (Exception ex) when (ex is not (BalancePreorderFailedException or ProductNotFoundException or InsufficientStockException or InsufficientBalanceException))
            {
                throw new BalancePreorderFailedException("An error occurred while processing the preorder request", ex);
            }
        }
    }
} 