using System;

namespace ECommerce.Application.Exceptions
{
    public class BalancePreorderFailedException : Exception
    {
        public BalancePreorderFailedException(string message) : base(message)
        {
        }

        public BalancePreorderFailedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 