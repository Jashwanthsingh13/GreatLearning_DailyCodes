using System;
using System.Collections.Generic;
using FinTech.Wallet.Interfaces;
using FinTech.Wallet.Services.Notifications;
using FinTech.Wallet.Services.PaymentProcessors;
using FinTech.Wallet.Services.Repository;

namespace FinTech.Wallet.DependencyInjection
{
    /// <summary>
    /// Service Provider for building and configuring wallet services
    /// Dependency Inversion: Centralizes dependency configuration
    /// Single Responsibility: Only handles service setup and configuration
    /// </summary>
    public class WalletServiceProvider
    {
        private readonly Dictionary<string, IPaymentProcessor> _paymentProcessors;
        private INotificationService _notificationService;
        private ITransactionRepository _transactionRepository;

        public WalletServiceProvider()
        {
            _paymentProcessors = new Dictionary<string, IPaymentProcessor>();
        }

        /// <summary>
        /// Register a payment processor
        /// Open/Closed Principle: Easy to add new payment methods
        /// </summary>
        public WalletServiceProvider RegisterPaymentProcessor(string name, IPaymentProcessor processor)
        {
            _paymentProcessors[name.ToUpper()] = processor;
            return this;
        }

        /// <summary>
        /// Register a notification service
        /// </summary>
        public WalletServiceProvider RegisterNotificationService(INotificationService service)
        {
            _notificationService = service;
            return this;
        }

        /// <summary>
        /// Register a transaction repository
        /// </summary>
        public WalletServiceProvider RegisterTransactionRepository(ITransactionRepository repository)
        {
            _transactionRepository = repository;
            return this;
        }

        /// <summary>
        /// Build wallet service with configured dependencies
        /// </summary>
        public Core.WalletService BuildWalletService(string userId, decimal initialBalance = 0)
        {
            // Use defaults if not configured
            _notificationService ??= new CompositeNotificationService(
                new EmailNotificationService(),
                new SMSNotificationService()
            );

            _transactionRepository ??= new InMemoryTransactionRepository();

            return new Core.WalletService(
                userId,
                initialBalance,
                _paymentProcessors,
                _notificationService,
                _transactionRepository
            );
        }
    }
}
