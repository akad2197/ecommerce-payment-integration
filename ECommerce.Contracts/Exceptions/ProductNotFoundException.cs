namespace ECommerce.Contracts.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string productId) 
            : base($"Product not found: {productId}")
        {
        }
    }
} 