using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerce.Tests.Handlers.Orders
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        public GetOrderByIdQueryHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
        }

        [Fact]
        public async Task Should_Return_Order_When_Found()
        {
            // Arrange
            var order = new Order 
            { 
                OrderId = "ORD-1",
                TotalAmount = 100,
                Currency = "USD",
                Status = "Pending",
                ProductIds = new List<string> { "P1", "P2" }
            };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync("ORD-1"))
                              .ReturnsAsync(order);

            var handler = new GetOrderByIdQueryHandler(_orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetOrderByIdQuery("ORD-1"), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.OrderId.Should().Be("ORD-1");
            result.TotalAmount.Should().Be(100);
            result.Currency.Should().Be("USD");
            result.Status.Should().Be("Pending");
            result.ProductIds.Should().BeEquivalentTo(new[] { "P1", "P2" });
        }

        [Fact]
        public async Task Should_Return_Null_When_Order_Not_Found()
        {
            // Arrange
            _orderRepositoryMock.Setup(x => x.GetByIdAsync("ORD-999"))
                              .ReturnsAsync((Order)null);

            var handler = new GetOrderByIdQueryHandler(_orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetOrderByIdQuery("ORD-999"), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_Call_Repository_With_Correct_Id()
        {
            // Arrange
            var orderId = "ORD-123";
            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId))
                              .ReturnsAsync(new Order { OrderId = orderId });

            var handler = new GetOrderByIdQueryHandler(_orderRepositoryMock.Object);

            // Act
            await handler.Handle(new GetOrderByIdQuery(orderId), CancellationToken.None);

            // Assert
            _orderRepositoryMock.Verify(x => x.GetByIdAsync(orderId), Times.Once);
        }
    }
} 