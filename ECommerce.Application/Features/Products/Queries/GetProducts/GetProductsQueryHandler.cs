using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<BalanceProductDto>>
    {
        private readonly IBalanceProductService _productService;

        public GetProductsQueryHandler(IBalanceProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<BalanceProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductsAsync();
        }
    }
} 