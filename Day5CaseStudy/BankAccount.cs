using System;

/// <summary>
/// Represents a bank account with deposit, withdrawal, and balance checking functionality.
/// Implements exception handling for invalid operations and maintains business rules.
/// </summary>
public class BankAccount
{
    // Constants
    private const double MINIMUM_BALANCE = 1000.0;

    // Properties
    public string AccountHolderName { get; private set; }
    public double Balance { get; private set; }

    /// <summary>
    /// Initializes a new instance of the BankAccount class.
    /// </summary>
    /// <param name="accountHolderName">Name of the account holder</param>
    /// <param name="initialBalance">Initial balance (must be >= 1000)</param>
    public BankAccount(string accountHolderName, double initialBalance)
    {
        if (string.IsNullOrWhiteSpace(accountHolderName))
        {
            throw new ArgumentException("Account holder name cannot be empty.");
        }

        if (initialBalance < MINIMUM_BALANCE)
        {
            throw new InvalidAmountException(
                $"Initial balance must be at least ₹{MINIMUM_BALANCE}.");
        }

        AccountHolderName = accountHolderName;
        Balance = initialBalance;
    }

    /// <summary>
    /// Deposits money into the account.
    /// </summary>
    /// <param name="amount">Amount to deposit (must be > 0)</param>
    /// <exception cref="InvalidAmountException">Thrown when amount <= 0</exception>
    public void Deposit(double amount)
    {
        try
        {
            if (amount <= 0)
            {
                throw new InvalidAmountException(
                    $"Deposit amount must be greater than 0. Provided: ₹{amount}");
            }

            Balance += amount;
            Console.WriteLine($"✓ Successfully deposited: ₹{amount:F2}");
            Console.WriteLine($"  Current Balance: ₹{Balance:F2}");
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"✗ Deposit Failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Unexpected error during deposit: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Withdraws money from the account.
    /// </summary>
    /// <param name="amount">Amount to withdraw (must be > 0 and not exceed balance)</param>
    /// <exception cref="InvalidAmountException">Thrown when amount <= 0</exception>
    /// <exception cref="InsufficientBalanceException">Thrown when withdrawal exceeds balance or results in balance below minimum</exception>
    public void Withdraw(double amount)
    {
        try
        {
            // Validate amount
            if (amount <= 0)
            {
                throw new InvalidAmountException(
                    $"Withdrawal amount must be greater than 0. Provided: ₹{amount}");
            }

            // Check if amount exceeds current balance
            if (amount > Balance)
            {
                throw new InsufficientBalanceException(
                    $"Withdrawal amount (₹{amount:F2}) exceeds available balance (₹{Balance:F2}).");
            }

            // Check if withdrawal would violate minimum balance rule
            double balanceAfterWithdrawal = Balance - amount;
            if (balanceAfterWithdrawal < MINIMUM_BALANCE)
            {
                throw new InsufficientBalanceException(
                    $"Withdrawal would result in balance below minimum (₹{MINIMUM_BALANCE}). " +
                    $"Current Balance: ₹{Balance:F2}, Minimum Required: ₹{MINIMUM_BALANCE}, " +
                    $"Maximum Withdrawal: ₹{Balance - MINIMUM_BALANCE:F2}");
            }

            Balance -= amount;
            Console.WriteLine($"✓ Successfully withdrawn: ₹{amount:F2}");
            Console.WriteLine($"  Current Balance: ₹{Balance:F2}");
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"✗ Withdrawal Failed: {ex.Message}");
            throw;
        }
        catch (InsufficientBalanceException ex)
        {
            Console.WriteLine($"✗ Withdrawal Failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Unexpected error during withdrawal: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Checks and displays the current balance.
    /// </summary>
    public void CheckBalance()
    {
        try
        {
            Console.WriteLine($"Account Holder: {AccountHolderName}");
            Console.WriteLine($"Current Balance: ₹{Balance:F2}");
            Console.WriteLine($"Minimum Balance Required: ₹{MINIMUM_BALANCE:F2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error checking balance: {ex.Message}");
            throw;
        }
    }
}
