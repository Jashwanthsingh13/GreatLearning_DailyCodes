using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTech.Wallet.Interfaces;
using FinTech.Wallet.Models;

namespace FinTech.Wallet.Services.Repository
{
    /// <summary>
    /// In-Memory Transaction Repository
    /// Single Responsibility: Only handles transaction storage
    /// Dependency Inversion: Implements ITransactionRepository interface
    /// Note: Replace with actual database in production
    /// </summary>
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly Dictionary<string, List<Transaction>> _transactionsByUser;
        private readonly object _lockObject = new object();

        public InMemoryTransactionRepository()
        {
            _transactionsByUser = new Dictionary<string, List<Transaction>>();
        }

        public async Task SaveTransactionAsync(Transaction transaction)
        {
            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    string userId = transaction.TransactionId.Split('-')[0];
                    
                    if (!_transactionsByUser.ContainsKey(userId))
                    {
                        _transactionsByUser[userId] = new List<Transaction>();
                    }

                    _transactionsByUser[userId].Add(transaction);
                }
            });
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId)
        {
            return await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    if (_transactionsByUser.ContainsKey(userId))
                    {
                        return _transactionsByUser[userId].ToList();
                    }
                    return new List<Transaction>();
                }
            });
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    if (!_transactionsByUser.ContainsKey(userId))
                        return new List<Transaction>();

                    return _transactionsByUser[userId]
                        .Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate)
                        .ToList();
                }
            });
        }

        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    foreach (var transactions in _transactionsByUser.Values)
                    {
                        var transaction = transactions.FirstOrDefault(t => t.TransactionId == transactionId);
                        if (transaction != null)
                            return transaction;
                    }
                    return null;
                }
            });
        }
    }
}
