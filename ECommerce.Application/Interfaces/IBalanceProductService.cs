using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;

namespace ECommerce.Application.Interfaces
{
    public interface IBalanceProductService
    {
        Task<List<BalanceProductDto>> GetProductsAsync();
    }
} 