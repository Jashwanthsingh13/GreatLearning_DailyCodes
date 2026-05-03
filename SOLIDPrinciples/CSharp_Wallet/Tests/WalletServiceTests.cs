using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FinTech.Wallet.Core;
using FinTech.Wallet.Interfaces;
using FinTech.Wallet.Models;
using FinTech.Wallet.Services.Repository;

namespace FinTech.Wallet.Tests
{
    /// <summary>
    /// Mock implementations for testing
    /// Dependency Inversion: Easy to create mocks because code depends on abstractions
    /// </summary>
    public class MockPaymentProcessor : IPaymentProcessor
    {
        private readonly bool _shouldSucceed;

        public string PaymentMethodName => "MOCK";

        public MockPaymentProcessor(bool shouldSucceed = true)
        {
            _shouldSucceed = shouldSucceed;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(10);
            return _shouldSucceed
                ? new PaymentResult(true, "Mock payment successful", $"MOCK-{Guid.NewGuid()}")
                : new PaymentResult(false, "Mock payment failed");
        }

        public bool ValidatePaymentDetails(PaymentRequest request)
        {
            return _shouldSucceed;
        }
    }

    public class MockNotificationService : INotificationService
    {
        public List<Notification> SentNotifications { get; } = new List<Notification>();

        public async Task SendAsync(Notification notification)
        {
            SentNotifications.Add(notification);
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Unit tests for Wallet Service
    /// Single Responsibility: Tests focus on specific behaviors
    /// </summary>
    public class WalletServiceTests
    {
        private readonly MockNotificationService _notificationService;
        private readonly InMemoryTransactionRepository _transactionRepository;

        public WalletServiceTests()
        {
            _notificationService = new MockNotificationService();
            _transactionRepository = new InMemoryTransactionRepository();
        }

        [Fact]
        public void GetBalance_ReturnsInitialBalance()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>();
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            var balance = wallet.GetBalance();

            // Assert
            Assert.Equal(1000, balance);
        }

        [Fact]
        public async Task AddMoney_IncreasesBalance()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>();
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            await wallet.AddMoneyAsync(500);

            // Assert
            Assert.Equal(1500, wallet.GetBalance());
        }

        [Fact]
        public async Task AddMoney_CreatesTransaction()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>();
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            await wallet.AddMoneyAsync(250, "Test Credit");

            // Assert
            var transactions = await wallet.GetTransactionHistoryAsync();
            Assert.NotEmpty(transactions);
            Assert.Single(transactions);
            
            var transaction = transactions.First();
            Assert.Equal(250, transaction.Amount);
            Assert.Equal(TransactionType.Credit, transaction.Type);
        }

        [Fact]
        public async Task AddMoney_SendsNotification()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>();
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            await wallet.AddMoneyAsync(500);

            // Assert
            Assert.NotEmpty(_notificationService.SentNotifications);
            Assert.Single(_notificationService.SentNotifications);
            Assert.Equal(NotificationType.WalletLoaded, 
                _notificationService.SentNotifications[0].Type);
        }

        [Fact]
        public async Task MakePayment_SuccessfullyProcessesPayment()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>
            {
                { "MOCK", new MockPaymentProcessor(shouldSucceed: true) }
            };
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            var result = await wallet.MakePaymentAsync("RECIPIENT", 200, "MOCK", "details");

            // Assert
            Assert.True(result);
            Assert.Equal(800, wallet.GetBalance());
        }

        [Fact]
        public async Task MakePayment_FailsWithInsufficientBalance()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>
            {
                { "MOCK", new MockPaymentProcessor(shouldSucceed: true) }
            };
            var wallet = new WalletService("USER001", 100, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            var result = await wallet.MakePaymentAsync("RECIPIENT", 200, "MOCK", "details");

            // Assert
            Assert.False(result);
            Assert.Equal(100, wallet.GetBalance());
        }

        [Fact]
        public async Task MakePayment_FailsWithUnsupportedPaymentMethod()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>();
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            var result = await wallet.MakePaymentAsync("RECIPIENT", 200, "UNKNOWN", "details");

            // Assert
            Assert.False(result);
            Assert.Equal(1000, wallet.GetBalance());
        }

        [Fact]
        public async Task MakePayment_CreatesTransactionOnSuccess()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>
            {
                { "MOCK", new MockPaymentProcessor(shouldSucceed: true) }
            };
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            await wallet.MakePaymentAsync("RECIPIENT", 300, "MOCK", "details");

            // Assert
            var transactions = await wallet.GetTransactionHistoryAsync();
            Assert.NotEmpty(transactions);
            
            var transaction = transactions.First();
            Assert.Equal(300, transaction.Amount);
            Assert.Equal(TransactionType.Debit, transaction.Type);
        }

        [Fact]
        public async Task GetTransactionHistory_ReturnsAllTransactions()
        {
            // Arrange
            var paymentProcessors = new Dictionary<string, IPaymentProcessor>
            {
                { "MOCK", new MockPaymentProcessor(shouldSucceed: true) }
            };
            var wallet = new WalletService("USER001", 1000, paymentProcessors, 
                _notificationService, _transactionRepository);

            // Act
            await wallet.AddMoneyAsync(500);
            await wallet.MakePaymentAsync("RECIPIENT", 200, "MOCK", "details");

            // Assert
            var transactions = await wallet.GetTransactionHistoryAsync();
            Assert.Equal(2, transactions.Count());
        }

        [Fact]
        public async Task PaymentProcessor_CanBeEasilySwapped()
        {
            // Arrange
            var successProcessor = new MockPaymentProcessor(shouldSucceed: true);
            var failureProcessor = new MockPaymentProcessor(shouldSucceed: false);

            var paymentProcessors1 = new Dictionary<string, IPaymentProcessor>
            {
                { "MOCK", successProcessor }
            };
            var wallet1 = new WalletService("USER001", 1000, paymentProcessors1, 
                _notificationService, _transactionRepository);

            var paymentProcessors2 = new Dictionary<string, IPaymentProcessor>
            {
                { "MOCK", failureProcessor }
            };
            var wallet2 = new WalletService("USER002", 1000, paymentProcessors2, 
                _notificationService, _transactionRepository);

            // Act
            var result1 = await wallet1.MakePaymentAsync("RECIPIENT", 200, "MOCK", "details");
            var result2 = await wallet2.MakePaymentAsync("RECIPIENT", 200, "MOCK", "details");

            // Assert
            Assert.True(result1);
            Assert.False(result2);
            // Demonstrates Open/Closed Principle: Same interface, different behaviors
        }
    }
}
