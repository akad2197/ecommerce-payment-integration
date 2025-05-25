namespace ECommerce.Contracts.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string productId, int available, int requested) 
            : base($"Insufficient stock for product {productId}. Available: {available}, Requested: {requested}")
        {
        }
    }
} 