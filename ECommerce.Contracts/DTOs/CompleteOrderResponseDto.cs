namespace ECommerce.Contracts.DTOs
{
    public class CompleteOrderResponseDto
    {
        public bool Success { get; set; }
        public CompleteOrderDataDto Data { get; set; }
    }

    public class CompleteOrderDataDto
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CompletedAt { get; set; }
    }
} 