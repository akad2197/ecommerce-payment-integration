using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using ECommerce.Contracts.Exceptions;
using ECommerce.Application.Features.Orders.Commands.CreateOrder;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerce.Tests.Handlers.Orders
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IBalanceOrderService> _balanceOrderServiceMock;
        private readonly Mock<IBalanceProductService> _balanceProductServiceMock;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _balanceOrderServiceMock = new Mock<IBalanceOrderService>();
            _balanceProductServiceMock = new Mock<IBalanceProductService>();
            _handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object, 
                _balanceOrderServiceMock.Object,
                _balanceProductServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateOrder()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = 2 },
                    new() { ProductId = "product-2", Quantity = 1 }
                }
            };

            var availableProducts = new List<BalanceProductDto>
            {
                new() { Id = "product-1", Price = 50.00m, Stock = 5 },
                new() { Id = "product-2", Price = 30.00m, Stock = 3 }
            };

            var balanceResponse = new BalanceResponseDto
            {
                Success = true,
                Data = new BalanceDataDto
                {
                    UserId = "user-123",
                    AvailableBalance = 200.00m,
                    Currency = "USD",
                    LastUpdated = DateTime.UtcNow
                }
            };

            var preorderResponse = new PreorderResponseDto
            {
                Success = true,
                Data = new PreorderDataDto
                {
                    OrderId = "order-123",
                    Status = "success",
                    TotalAmount = 130.00m,
                    Currency = "USD"
                }
            };

            _balanceProductServiceMock
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(availableProducts);

            _balanceOrderServiceMock
                .Setup(x => x.GetBalanceAsync())
                .ReturnsAsync(balanceResponse);

            _balanceOrderServiceMock
                .Setup(x => x.PreorderAsync(It.IsAny<PreorderRequestDto>()))
                .ReturnsAsync(preorderResponse);

            _orderRepositoryMock
                .Setup(x => x.SaveAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.OrderId, result.OrderId);
            Assert.Equal("success", result.Status);

            _orderRepositoryMock.Verify(
                x => x.SaveAsync(It.Is<Order>(o => 
                    o.OrderId == command.OrderId && 
                    o.Amount == 130.00m &&
                    o.Currency == "USD" &&
                    o.ProductIds.Contains("product-1") &&
                    o.ProductIds.Contains("product-2"))),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ShouldThrowException()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "non-existent", Quantity = 1 }
                }
            };

            var availableProducts = new List<BalanceProductDto>
            {
                new() { Id = "product-1", Price = 50.00m, Stock = 5 }
            };

            _balanceProductServiceMock
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(availableProducts);

            // Act & Assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InsufficientStock_ShouldThrowException()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = 10 }
                }
            };

            var availableProducts = new List<BalanceProductDto>
            {
                new() { Id = "product-1", Price = 50.00m, Stock = 5 }
            };

            _balanceProductServiceMock
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(availableProducts);

            // Act & Assert
            await Assert.ThrowsAsync<InsufficientStockException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InsufficientBalance_ShouldThrowException()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = 2 } // 2 * 50 = 100 TL
                }
            };

            var availableProducts = new List<BalanceProductDto>
            {
                new() { Id = "product-1", Price = 50.00m, Stock = 5 }
            };

            var balanceResponse = new BalanceResponseDto
            {
                Success = true,
                Data = new BalanceDataDto
                {
                    UserId = "user-123",
                    AvailableBalance = 50.00m, // Yetersiz bakiye
                    Currency = "USD",
                    LastUpdated = DateTime.UtcNow
                }
            };

            _balanceProductServiceMock
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(availableProducts);

            _balanceOrderServiceMock
                .Setup(x => x.GetBalanceAsync())
                .ReturnsAsync(balanceResponse);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InsufficientBalanceException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            Assert.Contains("Insufficient balance", exception.Message);
        }

        [Fact]
        public async Task Handle_BalanceServiceThrowsException_ShouldThrowBalancePreorderFailedException()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = 1 }
                }
            };

            var availableProducts = new List<BalanceProductDto>
            {
                new() { Id = "product-1", Price = 50.00m, Stock = 5 }
            };

            _balanceProductServiceMock
                .Setup(x => x.GetProductsAsync())
                .ReturnsAsync(availableProducts);

            _balanceOrderServiceMock
                .Setup(x => x.GetBalanceAsync())
                .ThrowsAsync(new BalanceApiException("Balance service error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BalancePreorderFailedException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            Assert.Contains("An error occurred while processing the preorder request", exception.Message);
        }
    }
} 