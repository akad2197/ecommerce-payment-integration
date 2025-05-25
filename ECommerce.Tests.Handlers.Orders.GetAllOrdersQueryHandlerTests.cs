using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Features.Orders.Queries.GetAllOrders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerce.Tests.Handlers.Orders
{
    public class GetAllOrdersQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        public GetAllOrdersQueryHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
        }

        [Fact]
        public async Task Should_Return_All_Orders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order 
                { 
                    OrderId = "ORD-1",
                    TotalAmount = 100,
                    Currency = "USD",
                    Status = "Pending",
                    ProductIds = new List<string> { "P1" }
                },
                new Order 
                { 
                    OrderId = "ORD-2",
                    TotalAmount = 200,
                    Currency = "EUR",
                    Status = "Completed",
                    ProductIds = new List<string> { "P2", "P3" }
                }
            };

            _orderRepositoryMock.Setup(x => x.GetAllAsync())
                              .ReturnsAsync(orders);

            var handler = new GetAllOrdersQueryHandler(_orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Select(x => x.OrderId).Should().Contain(new[] { "ORD-1", "ORD-2" });
            
            // İlk siparişin detaylarını kontrol et
            var firstOrder = result.First(x => x.OrderId == "ORD-1");
            firstOrder.TotalAmount.Should().Be(100);
            firstOrder.Currency.Should().Be("USD");
            firstOrder.Status.Should().Be("Pending");
            firstOrder.ProductIds.Should().ContainSingle("P1");

            // İkinci siparişin detaylarını kontrol et
            var secondOrder = result.First(x => x.OrderId == "ORD-2");
            secondOrder.TotalAmount.Should().Be(200);
            secondOrder.Currency.Should().Be("EUR");
            secondOrder.Status.Should().Be("Completed");
            secondOrder.ProductIds.Should().BeEquivalentTo(new[] { "P2", "P3" });
        }

        [Fact]
        public async Task Should_Return_EmptyList_When_NoOrders()
        {
            // Arrange
            _orderRepositoryMock.Setup(x => x.GetAllAsync())
                              .ReturnsAsync(new List<Order>());

            var handler = new GetAllOrdersQueryHandler(_orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Call_Repository_Once()
        {
            // Arrange
            _orderRepositoryMock.Setup(x => x.GetAllAsync())
                              .ReturnsAsync(new List<Order>());

            var handler = new GetAllOrdersQueryHandler(_orderRepositoryMock.Object);

            // Act
            await handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            _orderRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
} 