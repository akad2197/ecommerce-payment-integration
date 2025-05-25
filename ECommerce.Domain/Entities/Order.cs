using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public List<string> ProductIds { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 