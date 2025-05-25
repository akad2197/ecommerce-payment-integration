namespace ECommerce.Contracts.Exceptions
{
    public class BalanceDeserializationException : Exception
    {
        public BalanceDeserializationException(string message) : base(message)
        {
        }

        public BalanceDeserializationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 