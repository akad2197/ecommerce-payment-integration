namespace ECommerce.Contracts.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(decimal available, decimal required) 
            : base($"Insufficient balance. Available: {available}, Required: {required}")
        {
        }
    }
} 