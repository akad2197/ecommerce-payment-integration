using ECommerce.Contracts.DTOs;

namespace ECommerce.Application.Interfaces
{
    public interface IBalanceOrderService
    {
        Task<PreorderResponseDto> PreorderAsync(PreorderRequestDto request);
        Task<CompleteOrderResponseDto> CompleteAsync(string orderId);
        Task<BalanceResponseDto> GetBalanceAsync();
    }
} 