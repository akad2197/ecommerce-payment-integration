using System.Net.Http.Json;
using System.Text.Json;
using ECommerce.Application.Interfaces;
using ECommerce.Contracts.DTOs;
using ECommerce.Contracts.Exceptions;

namespace ECommerce.Infrastructure.External.Balance.Services
{
    public class BalanceOrderService : IBalanceOrderService, IBalanceProductService
    {
        private readonly HttpClient _httpClient;

        public BalanceOrderService(HttpClient httpClient)
        {
            Console.WriteLine("BaseUrl: " + httpClient.BaseAddress);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<BalanceProductDto>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/products");
                response.EnsureSuccessStatusCode();

                var productResponse = await response.Content.ReadFromJsonAsync<BalanceProductResponseDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (productResponse == null || !productResponse.Success)
                {
                    throw new Exception("Failed to get products from Balance Management API");
                }

                return productResponse.Data;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to fetch products from Balance Management API", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize products from Balance Management API", ex);
            }
        }

        public async Task<PreorderResponseDto> PreorderAsync(PreorderRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/preorder", request);
                response.EnsureSuccessStatusCode();

                var preorderResponse = await response.Content.ReadFromJsonAsync<PreorderResponseDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return preorderResponse ?? throw new Exception("Failed to deserialize preorder response");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to create preorder in Balance Management API", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize response from Balance Management API", ex);
            }
        }
        
        public async Task<CompleteOrderResponseDto> CompleteAsync(string orderId)
        {
            try
            {
                var request = new { OrderId = orderId };
                var response = await _httpClient.PostAsJsonAsync("/api/complete", request);
                response.EnsureSuccessStatusCode();

                var completeResponse = await response.Content.ReadFromJsonAsync<CompleteOrderResponseDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return completeResponse ?? throw new Exception("Failed to deserialize complete response");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to complete order in Balance Management API", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize response from Balance Management API", ex);
            }
        }

        public async Task<BalanceResponseDto> GetBalanceAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/balance");
                response.EnsureSuccessStatusCode();

                var balanceResponse = await response.Content.ReadFromJsonAsync<BalanceResponseDto>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return balanceResponse ?? throw new BalanceDeserializationException("Failed to deserialize balance response");
            }
            catch (HttpRequestException ex)
            {
                throw new BalanceApiException("Failed to fetch balance from Balance Management API", ex);
            }
            catch (JsonException ex)
            {
                throw new BalanceDeserializationException("Failed to deserialize response from Balance Management API", ex);
            }
        }
    }
} 