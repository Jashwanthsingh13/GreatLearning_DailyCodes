using System;

namespace Scenario3_BankingTransactionSystem
{
    /// <summary>
    /// Transaction class represents a financial transaction
    /// </summary>
    public class Transaction
    {
        public string TransactionId { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; } // Debit, Credit
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public Transaction(string transactionId, double amount, string type, string description = "")
        {
            TransactionId = transactionId;
            Amount = amount;
            Type = type;
            Description = description;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {TransactionId}, Amount: ${Amount}, Type: {Type}, Description: {Description}, Time: {Timestamp:dd/MM/yyyy HH:mm:ss}";
        }
    }
}
