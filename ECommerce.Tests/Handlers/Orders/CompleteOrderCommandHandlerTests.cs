using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using ECommerce.Contracts.Exceptions;
using ECommerce.Application.Features.Orders.Commands.CompleteOrder;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerce.Tests.Handlers.Orders
{
    public class CompleteOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IBalanceOrderService> _balanceOrderServiceMock;
        private readonly CompleteOrderCommandHandler _handler;

        public CompleteOrderCommandHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _balanceOrderServiceMock = new Mock<IBalanceOrderService>();
            _handler = new CompleteOrderCommandHandler(_orderRepositoryMock.Object, _balanceOrderServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCompleteOrder()
        {
            // Arrange
            var command = new CompleteOrderCommand { OrderId = "order-123" };
            var order = new Order { OrderId = "order-123" };

            var completeResponse = new CompleteOrderResponseDto
            {
                Success = true,
                Data = new CompleteOrderDataDto
                {
                    OrderId = "order-123",
                    Status = "completed",
                    Amount = 130.00m,
                    Currency = "USD",
                    CompletedAt = DateTime.UtcNow
                }
            };

            _orderRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(order);

            _balanceOrderServiceMock
                .Setup(x => x.CompleteAsync(It.IsAny<string>()))
                .ReturnsAsync(completeResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.OrderId, result.OrderId);
            Assert.Equal("completed", result.Status);

            _orderRepositoryMock.Verify(
                x => x.UpdateStatusAsync(command.OrderId, "completed"),
                Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ShouldThrowException()
        {
            // Arrange
            var command = new CompleteOrderCommand { OrderId = "order-123" };

            _orderRepositoryMock
                .Setup(x => x.GetByIdAsync(command.OrderId))
                .ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<OrderNotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_BalanceServiceThrowsException_ShouldThrowBalanceCompleteFailedException()
        {
            // Arrange
            var command = new CompleteOrderCommand { OrderId = "order-123" };
            var order = new Order { OrderId = "order-123" };

            _orderRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(order);

            _balanceOrderServiceMock
                .Setup(x => x.CompleteAsync(It.IsAny<string>()))
                .ThrowsAsync(new BalanceApiException("Balance service error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BalanceCompleteFailedException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            Assert.Contains("An error occurred while processing the complete request", exception.Message);
        }
    }
} 