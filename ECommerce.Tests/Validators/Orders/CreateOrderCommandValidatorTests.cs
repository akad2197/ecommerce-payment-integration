using System.Collections.Generic;
using ECommerce.Application.Features.Orders.Commands.CreateOrder;
using FluentAssertions;
using Xunit;

namespace ECommerce.Tests.Validators.Orders
{
    public class CreateOrderCommandValidatorTests
    {
        private readonly CreateOrderCommandValidator _validator;

        public CreateOrderCommandValidatorTests()
        {
            _validator = new CreateOrderCommandValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Fail_When_OrderId_Is_NullOrEmpty(string orderId)
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = orderId,
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = 1 }
                }
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "OrderId");
        }

        [Fact]
        public void Should_Fail_When_Products_Is_Null()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = null
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Products");
        }

        [Fact]
        public void Should_Fail_When_Products_Is_Empty()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>()
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Products");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Fail_When_ProductId_Is_NullOrEmpty(string productId)
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = productId, Quantity = 1 }
                }
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Products[0].ProductId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_Quantity_Is_LessThanOrEqualToZero(int quantity)
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = quantity }
                }
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Products[0].Quantity");
        }

        [Fact]
        public void Should_Pass_When_All_Properties_Are_Valid()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderId = "order-123",
                Products = new List<OrderProductDto>
                {
                    new() { ProductId = "product-1", Quantity = 1 },
                    new() { ProductId = "product-2", Quantity = 2 }
                }
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
} 