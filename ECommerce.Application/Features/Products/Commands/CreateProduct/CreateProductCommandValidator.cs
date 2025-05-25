using FluentValidation;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz")
                .MaximumLength(100).WithMessage("Ürün adı 100 karakterden uzun olamaz");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır")
                .LessThan(1000000).WithMessage("Fiyat 1.000.000'dan büyük olamaz");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Para birimi boş olamaz")
                .Length(3).WithMessage("Para birimi 3 karakter olmalıdır (örn: USD, EUR, TRY)")
                .Matches("^[A-Z]{3}$").WithMessage("Para birimi büyük harflerden oluşan 3 karakter olmalıdır");
        }
    }
} 