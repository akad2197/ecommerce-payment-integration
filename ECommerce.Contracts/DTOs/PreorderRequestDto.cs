namespace ECommerce.Contracts.DTOs
{
    public class PreorderRequestDto
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
} 