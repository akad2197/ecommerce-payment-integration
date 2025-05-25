using FluentValidation;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            // Şu an için validasyon kuralı yok, ama yapıyı göstermek için ekledik
            // İleride filtreleme, sayfalama gibi parametreler eklendiğinde validasyonlar eklenebilir
        }
    }
} 