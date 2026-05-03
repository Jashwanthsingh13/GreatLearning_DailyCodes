using System;
using System.Threading.Tasks;
using FinTech.Wallet.Interfaces;

namespace FinTech.Wallet.Services.PaymentProcessors
{
    /// <summary>
    /// UPI Payment Processor
    /// Open/Closed Principle: Easy to add without modifying existing code
    /// Single Responsibility: Only handles UPI payment logic
    /// </summary>
    public class UPIPaymentProcessor : IPaymentProcessor
    {
        public string PaymentMethodName => "UPI";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                // Validate payment details
                if (!ValidatePaymentDetails(request))
                    return new PaymentResult(false, "Invalid UPI details");

                // Simulate API call to UPI gateway
                await Task.Delay(500); // Simulate network delay

                // Mock success
                string reference = $"UPI-{Guid.NewGuid()}";
                return new PaymentResult(true, "Payment processed successfully via UPI", reference);
            }
            catch (Exception ex)
            {
                return new PaymentResult(false, $"UPI Payment failed: {ex.Message}");
            }
        }

        public bool ValidatePaymentDetails(PaymentRequest request)
        {
            // Validate UPI format (e.g., user@upi)
            if (string.IsNullOrEmpty(request.PaymentDetails))
                return false;

            return request.PaymentDetails.Contains("@") && request.Amount > 0;
        }
    }

    /// <summary>
    /// Card Payment Processor
    /// </summary>
    public class CardPaymentProcessor : IPaymentProcessor
    {
        public string PaymentMethodName => "Card";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                if (!ValidatePaymentDetails(request))
                    return new PaymentResult(false, "Invalid card details");

                // Simulate API call to Payment Gateway
                await Task.Delay(800);

                string reference = $"CARD-{Guid.NewGuid()}";
                return new PaymentResult(true, "Payment processed successfully via Card", reference);
            }
            catch (Exception ex)
            {
                return new PaymentResult(false, $"Card Payment failed: {ex.Message}");
            }
        }

        public bool ValidatePaymentDetails(PaymentRequest request)
        {
            // Validate card number (mock: 16 digits)
            if (string.IsNullOrEmpty(request.PaymentDetails) || request.PaymentDetails.Length < 16)
                return false;

            return request.Amount > 0;
        }
    }

    /// <summary>
    /// Net Banking Payment Processor
    /// </summary>
    public class NetBankingPaymentProcessor : IPaymentProcessor
    {
        public string PaymentMethodName => "NetBanking";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                if (!ValidatePaymentDetails(request))
                    return new PaymentResult(false, "Invalid bank details");

                // Simulate API call to Bank Gateway
                await Task.Delay(1000);

                string reference = $"NB-{Guid.NewGuid()}";
                return new PaymentResult(true, "Payment processed successfully via Net Banking", reference);
            }
            catch (Exception ex)
            {
                return new PaymentResult(false, $"Net Banking Payment failed: {ex.Message}");
            }
        }

        public bool ValidatePaymentDetails(PaymentRequest request)
        {
            if (string.IsNullOrEmpty(request.PaymentDetails))
                return false;

            return request.Amount > 0;
        }
    }
}
