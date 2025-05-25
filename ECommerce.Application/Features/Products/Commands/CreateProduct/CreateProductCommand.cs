using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<BalanceProductDto>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
} 