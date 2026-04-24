using System;

namespace Scenario3_BankingTransactionSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display system info
            BankingTransactionSystem.DisplaySystemInfo();

            // Create banking system
            BankingTransactionSystem bankSystem = new BankingTransactionSystem();

            // Create accounts
            bankSystem.CreateAccount(new BankAccount("ACC001", "John Doe", 5000, "Savings"));
            bankSystem.CreateAccount(new BankAccount("ACC002", "Jane Smith", 7500, "Checking"));
            bankSystem.CreateAccount(new BankAccount("ACC003", "Bob Johnson", 3000, "Savings"));

            // Display accounts
            bankSystem.DisplayAllAccounts();

            // Process transactions
            bankSystem.ProcessTransaction(new Transaction("TXN001", 500, "Credit", "Transfer from ACC002"));
            bankSystem.ProcessTransaction(new Transaction("TXN002", 200, "Debit", "Withdrawal"));
            bankSystem.ProcessTransaction(new Transaction("TXN003", 1500, "Credit", "Salary deposit"));
            bankSystem.ProcessTransaction(new Transaction("TXN004", 300, "Debit", "Bill payment"));
            bankSystem.ProcessTransaction(new Transaction("TXN005", 800, "Credit", "Refund"));

            // Try to process duplicate transaction
            bankSystem.ProcessTransaction(new Transaction("TXN001", 100, "Credit", "Duplicate attempt"));

            // Try to process transaction exceeding limit
            bankSystem.ProcessTransaction(new Transaction("TXN006", 150000, "Credit", "Large transfer"));

            // Display pending transactions
            Console.WriteLine("\n--- Pending Transactions ---");
            Console.WriteLine("Transactions queued and ready for execution\n");

            // Execute pending transactions
            bankSystem.ExecutePendingTransactions();

            // Update account balances
            Console.WriteLine("\n--- Updating Account Balances ---");
            bankSystem.UpdateBalance("ACC001", 500, false); // Credit
            bankSystem.UpdateBalance("ACC002", 200, true);  // Debit
            bankSystem.UpdateBalance("ACC003", 1500, false); // Credit

            // Display balances
            bankSystem.DisplayAccountBalances();

            // Display transaction history
            bankSystem.DisplayTransactionHistory();

            // Rollback operations
            Console.WriteLine("\n--- Rollback Operations ---");
            bankSystem.RollbackLastTransaction();
            bankSystem.RollbackLastTransaction();

            // Display final transaction history
            bankSystem.DisplayTransactionHistory();

            // Display statistics
            bankSystem.DisplayStatistics();
        }
    }
}
