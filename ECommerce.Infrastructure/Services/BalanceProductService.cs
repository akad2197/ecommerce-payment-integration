using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Contracts.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Contracts.DTOs;

namespace ECommerce.Infrastructure.Services
{
    public class BalanceProductService : IBalanceProductService
    {
        private readonly HttpClient _httpClient;

        public BalanceProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<BalanceProductDto>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/products");
                response.EnsureSuccessStatusCode();

                var products = await response.Content.ReadFromJsonAsync<List<BalanceProductDto>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return products ?? new List<BalanceProductDto>();
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
    }
} 