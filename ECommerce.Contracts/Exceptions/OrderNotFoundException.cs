namespace ECommerce.Contracts.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string orderId) 
            : base($"Order not found: {orderId}")
        {
        }
    }
} 