using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario3_BankingTransactionSystem
{
    /// <summary>
    /// Interface for banking operations
    /// </summary>
    public interface IBankingSystem
    {
        void ProcessTransaction(Transaction transaction);
        void RollbackLastTransaction();
        void DisplayTransactionHistory();
    }

    /// <summary>
    /// BankingTransactionSystem class implements IBankingSystem
    /// Manages transactions, accounts, and financial operations
    /// </summary>
    public class BankingTransactionSystem : IBankingSystem
    {
        // Constants
        private const string BANK_NAME = "Secure Banking System";
        private const double MIN_BALANCE = 0;
        private const double TRANSACTION_LIMIT = 100000;

        // Collections
        private List<Transaction> transactionHistory;          // Store transaction history
        private Dictionary<string, double> accountBalances;    // Store account balances
        private Queue<Transaction> pendingTransactions;        // Queue for pending transactions
        private Stack<Transaction> rollbackStack;              // Stack for rollback operations
        private HashSet<string> processedTransactionIds;       // Ensure unique transaction IDs
        private Dictionary<string, BankAccount> bankAccounts;  // Dictionary to store accounts

        public BankingTransactionSystem()
        {
            transactionHistory = new List<Transaction>();
            accountBalances = new Dictionary<string, double>();
            pendingTransactions = new Queue<Transaction>();
            rollbackStack = new Stack<Transaction>();
            processedTransactionIds = new HashSet<string>();
            bankAccounts = new Dictionary<string, BankAccount>();
        }

        // Static method to display system info
        public static void DisplaySystemInfo()
        {
            Console.WriteLine($"=== {BANK_NAME} ===");
            Console.WriteLine($"Min Balance: ${MIN_BALANCE}");
            Console.WriteLine($"Transaction Limit: ${TRANSACTION_LIMIT}");
            Console.WriteLine();
        }

        /// <summary>
        /// Create a new bank account
        /// </summary>
        public void CreateAccount(BankAccount account)
        {
            if (!bankAccounts.ContainsKey(account.AccountNumber))
            {
                bankAccounts.Add(account.AccountNumber, account);
                accountBalances[account.AccountNumber] = account.Balance;
                Console.WriteLine($"✓ Account created: {account.AccountHolder} ({account.AccountNumber})");
            }
            else
            {
                Console.WriteLine($"✗ Account {account.AccountNumber} already exists");
            }
        }

        /// <summary>
        /// Process a transaction
        /// </summary>
        public void ProcessTransaction(Transaction transaction)
        {
            // Validate transaction ID uniqueness
            if (processedTransactionIds.Contains(transaction.TransactionId))
            {
                Console.WriteLine($"✗ Transaction {transaction.TransactionId} already processed (duplicate)");
                return;
            }

            // Validate transaction amount
            if (transaction.Amount <= 0)
            {
                Console.WriteLine($"✗ Invalid transaction amount: ${transaction.Amount}");
                return;
            }

            // Validate transaction limit
            if (transaction.Amount > TRANSACTION_LIMIT)
            {
                Console.WriteLine($"✗ Transaction exceeds limit (${TRANSACTION_LIMIT})");
                return;
            }

            // Add to pending queue
            pendingTransactions.Enqueue(transaction);
            processedTransactionIds.Add(transaction.TransactionId);
            Console.WriteLine($"✓ Transaction queued: {transaction.TransactionId} - ${transaction.Amount}");
        }

        /// <summary>
        /// Execute pending transactions
        /// </summary>
        public void ExecutePendingTransactions()
        {
            Console.WriteLine("\n--- Executing Pending Transactions ---");
            int executedCount = 0;

            while (pendingTransactions.Count > 0)
            {
                Transaction transaction = pendingTransactions.Dequeue();

                // Find account (this is simplified - in real system would need account info)
                bool isValid = true;

                if (isValid)
                {
                    transactionHistory.Add(transaction);
                    rollbackStack.Push(transaction);
                    Console.WriteLine($"✓ Executed: {transaction}");
                    executedCount++;
                }
                else
                {
                    Console.WriteLine($"✗ Failed to execute: {transaction.TransactionId}");
                }
            }
            Console.WriteLine($"Total executed: {executedCount}\n");
        }

        /// <summary>
        /// Rollback last transaction
        /// </summary>
        public void RollbackLastTransaction()
        {
            if (rollbackStack.Count > 0)
            {
                Transaction transaction = rollbackStack.Pop();
                transactionHistory.Remove(transaction);
                processedTransactionIds.Remove(transaction.TransactionId);
                Console.WriteLine($"✓ Rolled back: {transaction.TransactionId} - ${transaction.Amount}");
            }
            else
            {
                Console.WriteLine("✗ No transactions to rollback");
            }
        }

        /// <summary>
        /// Update account balance
        /// </summary>
        public void UpdateBalance(string accountNumber, double amount, bool isDebit = false)
        {
            if (accountBalances.ContainsKey(accountNumber))
            {
                if (isDebit)
                {
                    if (accountBalances[accountNumber] >= amount)
                    {
                        accountBalances[accountNumber] -= amount;
                        Console.WriteLine($"✓ Debit: ${amount} from {accountNumber}. New Balance: ${accountBalances[accountNumber]}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Insufficient funds in {accountNumber}");
                    }
                }
                else
                {
                    accountBalances[accountNumber] += amount;
                    Console.WriteLine($"✓ Credit: ${amount} to {accountNumber}. New Balance: ${accountBalances[accountNumber]}");
                }
            }
            else
            {
                Console.WriteLine($"✗ Account {accountNumber} not found");
            }
        }

        /// <summary>
        /// Display transaction history
        /// </summary>
        public void DisplayTransactionHistory()
        {
            Console.WriteLine("\n--- Transaction History ---");
            if (transactionHistory.Count == 0)
            {
                Console.WriteLine("No transactions found");
                return;
            }

            for (int i = 0; i < transactionHistory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {transactionHistory[i]}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all accounts
        /// </summary>
        public void DisplayAllAccounts()
        {
            Console.WriteLine("\n--- All Accounts ---");
            if (bankAccounts.Count == 0)
            {
                Console.WriteLine("No accounts found");
                return;
            }

            foreach (var account in bankAccounts.Values)
            {
                Console.WriteLine(account);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display account balances
        /// </summary>
        public void DisplayAccountBalances()
        {
            Console.WriteLine("\n--- Account Balances ---");
            if (accountBalances.Count == 0)
            {
                Console.WriteLine("No accounts found");
                return;
            }

            foreach (var balance in accountBalances)
            {
                Console.WriteLine($"Account {balance.Key}: ${balance.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display system statistics
        /// </summary>
        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== Banking System Statistics ===");
            Console.WriteLine($"Total Accounts: {bankAccounts.Count}");
            Console.WriteLine($"Processed Transactions: {processedTransactionIds.Count}");
            Console.WriteLine($"Executed Transactions: {transactionHistory.Count}");
            Console.WriteLine($"Pending Transactions: {pendingTransactions.Count}");
            Console.WriteLine($"Total Balance Across Accounts: ${accountBalances.Values.Sum()}");
            Console.WriteLine();
        }

        public int GetTransactionCount() => transactionHistory.Count;
        public int GetAccountCount() => bankAccounts.Count;
    }
}
