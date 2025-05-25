using System.Collections.Generic;

namespace ECommerce.Infrastructure.External.Balance.Dtos
{
    public class PreorderRequestDto
    {
        public string OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public List<string> ProductIds { get; set; }
    }
} 