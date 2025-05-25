namespace ECommerce.Contracts.DTOs
{
    public class BalanceResponseDto
    {
        public bool Success { get; set; }
        public BalanceDataDto Data { get; set; }
    }

    public class BalanceDataDto
    {
        public string UserId { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal BlockedBalance { get; set; }
        public string Currency { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 