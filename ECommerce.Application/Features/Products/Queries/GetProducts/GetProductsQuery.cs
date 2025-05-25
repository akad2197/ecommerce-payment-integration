using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<BalanceProductDto>>
    {
    }
} 