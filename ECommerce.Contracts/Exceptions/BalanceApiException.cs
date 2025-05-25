namespace ECommerce.Contracts.Exceptions
{
    public class BalanceApiException : Exception
    {
        public BalanceApiException(string message) : base(message)
        {
        }

        public BalanceApiException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 