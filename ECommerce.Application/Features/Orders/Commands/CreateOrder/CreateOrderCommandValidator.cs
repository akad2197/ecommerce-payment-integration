using FluentValidation;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required");

            RuleFor(x => x.Products)
                .NotEmpty().WithMessage("At least one product is required")
                .Must(products => products != null && products.All(p => !string.IsNullOrEmpty(p.ProductId)))
                .WithMessage("All products must have a valid ProductId")
                .Must(products => products != null && products.All(p => p.Quantity > 0))
                .WithMessage("All products must have a quantity greater than 0");

            RuleForEach(x => x.Products)
                .ChildRules(product =>
                {
                    product.RuleFor(x => x.ProductId)
                        .NotEmpty().WithMessage("ProductId is required");

                    product.RuleFor(x => x.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than 0");
                });
        }
    }
} 