using FluentValidation;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .NotNull().WithMessage("Sipariş ID'si null olamaz")
                .NotEmpty().WithMessage("Sipariş ID'si boş olamaz");
        }
    }
} 