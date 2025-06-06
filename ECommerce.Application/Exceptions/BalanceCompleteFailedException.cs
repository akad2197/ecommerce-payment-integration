using System;

namespace ECommerce.Application.Exceptions
{
    public class BalanceCompleteFailedException : Exception
    {
        public BalanceCompleteFailedException(string message) : base(message)
        {
        }

        public BalanceCompleteFailedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
} 