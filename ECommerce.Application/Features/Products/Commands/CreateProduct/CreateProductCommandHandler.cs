using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BalanceProductDto>
    {
        private readonly IBalanceProductService _productService;

        public CreateProductCommandHandler(IBalanceProductService productService)
        {
            _productService = productService;
        }

        public async Task<BalanceProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Burada IBalanceProductService'e yeni ürün oluşturma metodu eklenmeli
            // Şimdilik sadece yapıyı gösteriyoruz
            return new BalanceProductDto
            {
                Name = request.Name,
                Price = request.Price,
                Currency = request.Currency
            };
        }
    }
} 