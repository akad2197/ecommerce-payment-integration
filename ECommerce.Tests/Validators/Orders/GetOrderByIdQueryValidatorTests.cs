using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;

namespace ECommerce.Tests.Validators.Orders
{
    public class GetOrderByIdQueryValidatorTests
    {
        private readonly GetOrderByIdQueryValidator _validator;

        public GetOrderByIdQueryValidatorTests()
        {
            _validator = new GetOrderByIdQueryValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_OrderId_Is_NullOrEmpty(string invalidId)
        {
            // Arrange
            var query = new GetOrderByIdQuery{OrderId = invalidId};

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderId)
                  .WithErrorMessage("Sipariş ID'si boş olamaz");
        }

        [Fact]
        public void Should_Pass_When_OrderId_Is_Valid()
        {
            // Arran
            var query = new GetOrderByIdQuery{OrderId = "ORD-123"};

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_OrderId_Is_Whitespace()
        {
            // Arrange
            var query = new GetOrderByIdQuery{OrderId = "   "};

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderId)
                  .WithErrorMessage("Sipariş ID'si boş olamaz");
        }
    }
} 