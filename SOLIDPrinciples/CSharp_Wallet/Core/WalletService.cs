using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTech.Wallet.Interfaces;
using FinTech.Wallet.Models;

namespace FinTech.Wallet.Core
{
    /// <summary>
    /// Main Wallet Service
    /// Single Responsibility: Manages wallet operations (balance, transactions)
    /// Dependency Inversion: Depends on abstractions (IPaymentProcessor, INotificationService, ITransactionRepository)
    /// This ensures low coupling and high cohesion
    /// </summary>
    public class WalletService
    {
        private decimal _balance;
        private readonly string _userId;
        private readonly Dictionary<string, IPaymentProcessor> _paymentProcessors;
        private readonly INotificationService _notificationService;
        private readonly ITransactionRepository _transactionRepository;

        public WalletService(
            string userId,
            decimal initialBalance,
            Dictionary<string, IPaymentProcessor> paymentProcessors,
            INotificationService notificationService,
            ITransactionRepository transactionRepository)
        {
            _userId = userId;
            _balance = initialBalance;
            _paymentProcessors = paymentProcessors ?? new Dictionary<string, IPaymentProcessor>();
            _notificationService = notificationService;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Get current wallet balance
        /// </summary>
        public decimal GetBalance()
        {
            return _balance;
        }

        /// <summary>
        /// Add money to wallet
        /// </summary>
        public async Task<bool> AddMoneyAsync(decimal amount, string description = "")
        {
            if (amount <= 0)
            {
                await NotifyUserAsync("Add Money Failed", "Amount must be greater than zero", 
                    NotificationType.TransactionFailure);
                return false;
            }

            _balance += amount;

            // Create and save transaction
            var transaction = new Transaction(
                $"{_userId}-{Guid.NewGuid()}",
                amount,
                TransactionType.Credit,
                PaymentMethod.Wallet,
                description
            );

            await _transactionRepository.SaveTransactionAsync(transaction);

            // Send notification
            await NotifyUserAsync("Wallet Loaded", 
                $"₹{amount:F2} added successfully. New Balance: ₹{_balance:F2}", 
                NotificationType.WalletLoaded);

            return true;
        }

        /// <summary>
        /// Make a payment using specified payment method
        /// Open/Closed Principle: Adding new payment types doesn't require modifying this method
        /// </summary>
        public async Task<bool> MakePaymentAsync(string recipientId, decimal amount, 
            string paymentMethod, string paymentDetails)
        {
            // Validate payment method
            if (!_paymentProcessors.ContainsKey(paymentMethod))
            {
                await NotifyUserAsync("Payment Failed", 
                    $"Payment method '{paymentMethod}' not supported", 
                    NotificationType.TransactionFailure);
                return false;
            }

            // Check sufficient balance
            if (_balance < amount)
            {
                await NotifyUserAsync("Payment Failed", 
                    "Insufficient balance", 
                    NotificationType.TransactionFailure);
                return false;
            }

            var processor = _paymentProcessors[paymentMethod];
            var paymentRequest = new PaymentRequest(amount, recipientId, paymentDetails, 
                $"{_userId}-{Guid.NewGuid()}");

            // Process payment
            var result = await processor.ProcessPaymentAsync(paymentRequest);

            if (!result.IsSuccess)
            {
                await NotifyUserAsync("Payment Failed", result.Message, 
                    NotificationType.TransactionFailure);
                return false;
            }

            // Deduct amount from wallet
            _balance -= amount;

            // Create and save transaction
            var transaction = new Transaction(
                paymentRequest.TransactionId,
                amount,
                TransactionType.Debit,
                GetPaymentMethodEnum(paymentMethod),
                $"Payment to {recipientId}"
            );

            await _transactionRepository.SaveTransactionAsync(transaction);

            // Send confirmation notification
            await NotifyUserAsync("Payment Confirmed", 
                $"₹{amount:F2} transferred to {recipientId} via {paymentMethod}. Ref: {result.TransactionReference}", 
                NotificationType.TransactionConfirmation);

            return true;
        }

        /// <summary>
        /// Get transaction history
        /// </summary>
        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync()
        {
            return await _transactionRepository.GetTransactionsByUserIdAsync(_userId);
        }

        /// <summary>
        /// Get transactions within date range
        /// </summary>
        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(DateTime startDate, DateTime endDate)
        {
            return await _transactionRepository.GetTransactionsByDateRangeAsync(_userId, startDate, endDate);
        }

        /// <summary>
        /// Send notification to user
        /// </summary>
        private async Task NotifyUserAsync(string title, string message, NotificationType type)
        {
            var notification = new Notification(_userId, title, message, type);
            await _notificationService.SendAsync(notification);
        }

        /// <summary>
        /// Helper method to convert string to PaymentMethod enum
        /// </summary>
        private PaymentMethod GetPaymentMethodEnum(string paymentMethod)
        {
            return paymentMethod.ToUpper() switch
            {
                "UPI" => PaymentMethod.UPI,
                "CARD" => PaymentMethod.Card,
                "NETBANKING" => PaymentMethod.NetBanking,
                _ => PaymentMethod.Wallet
            };
        }
    }
}
