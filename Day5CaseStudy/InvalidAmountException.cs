using System;

/// <summary>
/// Custom exception thrown when an invalid amount is provided for deposit or withdrawal.
/// Valid amount must be greater than 0.
/// </summary>
public class InvalidAmountException : Exception
{
    public InvalidAmountException(string message) : base(message)
    {
    }

    public InvalidAmountException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public InvalidAmountException() : base("Invalid amount. Amount must be greater than 0.")
    {
    }
}
