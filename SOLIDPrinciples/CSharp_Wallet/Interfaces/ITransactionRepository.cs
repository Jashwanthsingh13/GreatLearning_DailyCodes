using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTech.Wallet.Models;

namespace FinTech.Wallet.Interfaces
{
    /// <summary>
    /// Interface for transaction storage and retrieval
    /// Single Responsibility: Only handles transaction persistence
    /// Dependency Inversion: Depend on abstraction (Repository pattern)
    /// </summary>
    public interface ITransactionRepository
    {
        Task SaveTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate);
        Task<Transaction> GetTransactionByIdAsync(string transactionId);
    }
}
