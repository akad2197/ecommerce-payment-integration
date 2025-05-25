using FluentValidation;

namespace ECommerce.Application.Features.Orders.Commands.CompleteOrder
{
    public class CompleteOrderCommandValidator : AbstractValidator<CompleteOrderCommand>
    {
        public CompleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotNull().WithMessage("Sipariş ID'si null olamaz")
                .NotEmpty().WithMessage("Sipariş ID'si boş olamaz");
        }
    }
} 