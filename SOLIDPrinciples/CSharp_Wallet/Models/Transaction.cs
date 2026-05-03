using System;

namespace FinTech.Wallet.Models
{
    /// <summary>
    /// Represents a transaction in the wallet system.
    /// Single Responsibility: Only handles transaction data
    /// </summary>
    public class Transaction
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime Timestamp { get; set; }
        public TransactionStatus Status { get; set; }
        public string Description { get; set; }

        public Transaction(string transactionId, decimal amount, TransactionType type, 
                          PaymentMethod paymentMethod, string description = "")
        {
            TransactionId = transactionId;
            Amount = amount;
            Type = type;
            PaymentMethod = paymentMethod;
            Description = description;
            Timestamp = DateTime.Now;
            Status = TransactionStatus.Completed;
        }
    }

    public enum TransactionType
    {
        Credit,
        Debit
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum PaymentMethod
    {
        UPI,
        Card,
        NetBanking,
        Wallet
    }
}
