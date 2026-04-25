using System;

/// <summary>
/// Custom exception thrown when a withdrawal would result in balance
/// falling below the minimum balance of ₹1000 or exceeds available balance.
/// </summary>
public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string message) : base(message)
    {
    }

    public InsufficientBalanceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public InsufficientBalanceException() : base("Insufficient balance for this transaction.")
    {
    }
}
