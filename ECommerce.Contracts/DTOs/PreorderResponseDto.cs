namespace ECommerce.Contracts.DTOs
{
    public class PreorderResponseDto
    {
        public bool Success { get; set; }
        public PreorderDataDto Data { get; set; }
    }

    public class PreorderDataDto
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
    }
} 