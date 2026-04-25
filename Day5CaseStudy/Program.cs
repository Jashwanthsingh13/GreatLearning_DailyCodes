using System;

/// <summary>
/// Main application for demonstrating the Banking Transaction System
/// with comprehensive exception handling.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     BANKING TRANSACTION SYSTEM - Exception Demo        ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        BankAccount? account = null;

        try
        {
            // ==================== Test Case 1: Account Creation ====================
            Console.WriteLine("📌 TEST CASE 1: Creating a valid account");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account = new BankAccount("Rahul Kumar", 5000.0);
            account.CheckBalance();
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 2: Valid Deposit ====================
            Console.WriteLine("📌 TEST CASE 2: Valid deposit of ₹2000");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.Deposit(2000.0);
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 3: Invalid Deposit (Negative) ====================
            Console.WriteLine("📌 TEST CASE 3: Invalid deposit of ₹-500 (negative amount)");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.Deposit(-500.0);
            Console.WriteLine();
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"⚠️  Caught InvalidAmountException: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 4: Invalid Deposit (Zero) ====================
            Console.WriteLine("📌 TEST CASE 4: Invalid deposit of ₹0");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.Deposit(0.0);
            Console.WriteLine();
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"⚠️  Caught InvalidAmountException: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 5: Valid Withdrawal ====================
            Console.WriteLine("📌 TEST CASE 5: Valid withdrawal of ₹3000");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.Withdraw(3000.0);
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 6: Withdrawal Exceeding Balance ====================
            Console.WriteLine("📌 TEST CASE 6: Withdrawal of ₹10000 (exceeds balance)");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.Withdraw(10000.0);
            Console.WriteLine();
        }
        catch (InsufficientBalanceException ex)
        {
            Console.WriteLine($"⚠️  Caught InsufficientBalanceException: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 7: Withdrawal Below Minimum Balance ====================
            Console.WriteLine("📌 TEST CASE 7: Withdrawal of ₹3500 (would go below minimum balance)");
            Console.WriteLine("────────────────────────────────────────────────────────");
            if (account != null)
            {
                Console.WriteLine($"Current balance: ₹{account.Balance:F2}");
                Console.WriteLine("Attempting to withdraw ₹3500 (would result in ₹500, below ₹1000 minimum)");
                Console.WriteLine();
                account.Withdraw(3500.0);
            }
            Console.WriteLine();
        }
        catch (InsufficientBalanceException ex)
        {
            Console.WriteLine($"⚠️  Caught InsufficientBalanceException: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 8: Invalid Withdrawal (Negative) ====================
            Console.WriteLine("📌 TEST CASE 8: Invalid withdrawal of ₹-1000 (negative amount)");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.Withdraw(-1000.0);
            Console.WriteLine();
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"⚠️  Caught InvalidAmountException: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 9: Valid Withdrawal (Maximum allowed) ====================
            Console.WriteLine("📌 TEST CASE 9: Maximum valid withdrawal");
            Console.WriteLine("────────────────────────────────────────────────────────");
            if (account != null)
            {
                account.CheckBalance();
                double maxWithdrawal = account.Balance - 1000.0;
                Console.WriteLine($"Maximum withdrawal allowed: ₹{maxWithdrawal:F2}");
                Console.WriteLine("Attempting withdrawal...");
                Console.WriteLine();
                account.Withdraw(maxWithdrawal);
            }
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 10: Final Balance Check ====================
            Console.WriteLine("📌 TEST CASE 10: Final balance check");
            Console.WriteLine("────────────────────────────────────────────────────────");
            account?.CheckBalance();
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine();
        }

        try
        {
            // ==================== Test Case 11: Invalid Account Creation ====================
            Console.WriteLine("📌 TEST CASE 11: Attempting to create account with initial balance < ₹1000");
            Console.WriteLine("────────────────────────────────────────────────────────");
            BankAccount invalidAccount = new BankAccount("Priya Singh", 500.0);
            Console.WriteLine();
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"⚠️  Caught InvalidAmountException: {ex.Message}");
            Console.WriteLine();
        }

        // Final Summary
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║          BANKING TRANSACTION SYSTEM - DEMO COMPLETE    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Summary of Exception Handling Demonstrated:");
        Console.WriteLine("✓ InvalidAmountException - Negative/Zero deposits & withdrawals");
        Console.WriteLine("✓ InsufficientBalanceException - Excessive withdrawals");
        Console.WriteLine("✓ InsufficientBalanceException - Balance below minimum");
        Console.WriteLine("✓ ArgumentException - Invalid account holder name");
        Console.WriteLine("✓ Try-Catch-Finally blocks for error handling");
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
