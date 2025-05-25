using System;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;
using ECommerce.Application.Features.Orders.Commands.CompleteOrder;

namespace ECommerce.Tests.Validators.Orders
{
    public class CompleteOrderCommandValidatorTests
    {
        private readonly CompleteOrderCommandValidator _validator;

        public CompleteOrderCommandValidatorTests()
        {
            _validator = new CompleteOrderCommandValidator();
        }

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CompleteOrderCommand { OrderId = "ORD-123" };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_OrderId_Is_EmptyOrNull(string invalidOrderId)
        {
            // Arrange
            var command = new CompleteOrderCommand { OrderId = invalidOrderId };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderId)
                  .WithErrorMessage("Sipariş ID'si boş olamaz");
        }

        [Fact]
        public void Should_Fail_When_OrderId_Is_Whitespace()
        {
            // Arrange
            var command = new CompleteOrderCommand { OrderId = "   " };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderId)
                  .WithErrorMessage("Sipariş ID'si boş olamaz");
        }
    }
} 