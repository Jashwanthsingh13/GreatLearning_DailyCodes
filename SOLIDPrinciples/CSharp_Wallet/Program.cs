using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTech.Wallet.Core;
using FinTech.Wallet.DependencyInjection;
using FinTech.Wallet.Interfaces;
using FinTech.Wallet.Services.Notifications;
using FinTech.Wallet.Services.PaymentProcessors;

namespace FinTech.Wallet.Demo
{
    /// <summary>
    /// Demo application showcasing the SOLID principles-compliant Digital Wallet system
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           FinTech Digital Wallet System (C#)              ║");
            Console.WriteLine("║          Following SOLID Principles                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Setup: Configure wallet service with dependencies
            var walletService = SetupWalletService("USER001", 5000);

            Console.WriteLine("═══════════════════════════════════════════════════════════════\n");
            Console.WriteLine("Initial Balance: ₹5000.00\n");

            try
            {
                // 1. Add money to wallet
                Console.WriteLine("▶ Step 1: Adding money to wallet...");
                await walletService.AddMoneyAsync(2000, "Salary Credit");
                Console.WriteLine($"✓ Current Balance: ₹{walletService.GetBalance():F2}\n");

                await Task.Delay(1000);

                // 2. Make UPI Payment
                Console.WriteLine("▶ Step 2: Making payment via UPI...");
                bool upiSuccess = await walletService.MakePaymentAsync(
                    "user@bank",
                    500,
                    "UPI",
                    "merchant@upi"
                );
                Console.WriteLine($"✓ UPI Payment {(upiSuccess ? "Successful" : "Failed")}");
                Console.WriteLine($"✓ Current Balance: ₹{walletService.GetBalance():F2}\n");

                await Task.Delay(1000);

                // 3. Make Card Payment
                Console.WriteLine("▶ Step 3: Making payment via Card...");
                bool cardSuccess = await walletService.MakePaymentAsync(
                    "shopkeeper@retail",
                    1500,
                    "CARD",
                    "4111111111111111"
                );
                Console.WriteLine($"✓ Card Payment {(cardSuccess ? "Successful" : "Failed")}");
                Console.WriteLine($"✓ Current Balance: ₹{walletService.GetBalance():F2}\n");

                await Task.Delay(1000);

                // 4. Make Net Banking Payment
                Console.WriteLine("▶ Step 4: Making payment via Net Banking...");
                bool netBankingSuccess = await walletService.MakePaymentAsync(
                    "utility@bills",
                    800,
                    "NETBANKING",
                    "ACC123456789"
                );
                Console.WriteLine($"✓ Net Banking Payment {(netBankingSuccess ? "Successful" : "Failed")}");
                Console.WriteLine($"✓ Current Balance: ₹{walletService.GetBalance():F2}\n");

                await Task.Delay(1000);

                // 5. Try invalid payment (insufficient balance)
                Console.WriteLine("▶ Step 5: Attempting payment with insufficient balance...");
                await walletService.MakePaymentAsync(
                    "vendor@shop",
                    10000,
                    "UPI",
                    "vendor@upi"
                );
                Console.WriteLine($"✓ Current Balance: ₹{walletService.GetBalance():F2}\n");

                await Task.Delay(1000);

                // 6. View Transaction History
                Console.WriteLine("═══════════════════════════════════════════════════════════════\n");
                Console.WriteLine("▶ Step 6: Transaction History:\n");
                var transactions = await walletService.GetTransactionHistoryAsync();

                Console.WriteLine("Timestamp          | Type      | Amount  | Method      | Status");
                Console.WriteLine("─────────────────────────────────────────────────────────────");

                foreach (var transaction in transactions)
                {
                    Console.WriteLine(
                        $"{transaction.Timestamp:yyyy-MM-dd HH:mm:ss} | " +
                        $"{transaction.Type,-9} | ₹{transaction.Amount,7:F2} | " +
                        $"{transaction.PaymentMethod,-11} | {transaction.Status}"
                    );
                }

                Console.WriteLine("\n═══════════════════════════════════════════════════════════════\n");
                Console.WriteLine($"Final Balance: ₹{walletService.GetBalance():F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    SOLID Principles Applied                ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ S: Single Responsibility                                   ║");
            Console.WriteLine("║   - WalletService: Only manages wallet state & operations ║");
            Console.WriteLine("║   - PaymentProcessors: Each handles one payment type      ║");
            Console.WriteLine("║   - NotificationServices: Each handles one channel        ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║ O: Open/Closed Principle                                   ║");
            Console.WriteLine("║   - New payment types can be added without modification   ║");
            Console.WriteLine("║   - New notification channels easily extensible          ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║ L: Liskov Substitution Principle                          ║");
            Console.WriteLine("║   - All PaymentProcessor implementations are substitutable║");
            Console.WriteLine("║   - All NotificationService implementations work the same ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║ I: Interface Segregation Principle                        ║");
            Console.WriteLine("║   - Clients depend only on needed interfaces              ║");
            Console.WriteLine("║   - No forced implementation of unused methods            ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║ D: Dependency Inversion Principle                         ║");
            Console.WriteLine("║   - High-level modules depend on abstractions             ║");
            Console.WriteLine("║   - WalletServiceProvider handles dependency injection    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

            Console.WriteLine("\n✓ Demo completed successfully!");
        }

        /// <summary>
        /// Setup and configure the wallet service with all dependencies
        /// </summary>
        private static WalletService SetupWalletService(string userId, decimal initialBalance)
        {
            var provider = new WalletServiceProvider();

            // Register payment processors (easily extensible)
            provider
                .RegisterPaymentProcessor("UPI", new UPIPaymentProcessor())
                .RegisterPaymentProcessor("CARD", new CardPaymentProcessor())
                .RegisterPaymentProcessor("NETBANKING", new NetBankingPaymentProcessor());

            // Register notification services (can use multiple channels)
            var notificationService = new CompositeNotificationService(
                new EmailNotificationService(),
                new SMSNotificationService(),
                new PushNotificationService()
            );

            provider.RegisterNotificationService(notificationService);

            // Build and return the wallet service
            return provider.BuildWalletService(userId, initialBalance);
        }
    }
}
