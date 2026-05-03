using System.Threading.Tasks;

namespace FinTech.Wallet.Interfaces
{
    /// <summary>
    /// Interface for payment processing
    /// Open/Closed Principle: Open for extension (new payment types), closed for modification
    /// Dependency Inversion: Depend on abstraction, not concrete implementations
    /// </summary>
    public interface IPaymentProcessor
    {
        string PaymentMethodName { get; }
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        bool ValidatePaymentDetails(PaymentRequest request);
    }

    /// <summary>
    /// Request for processing payment
    /// </summary>
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string RecipientId { get; set; }
        public string PaymentDetails { get; set; }
        public string TransactionId { get; set; }

        public PaymentRequest(decimal amount, string recipientId, string paymentDetails, string transactionId)
        {
            Amount = amount;
            RecipientId = recipientId;
            PaymentDetails = paymentDetails;
            TransactionId = transactionId;
        }
    }

    /// <summary>
    /// Result of payment processing
    /// </summary>
    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string TransactionReference { get; set; }

        public PaymentResult(bool isSuccess, string message, string transactionReference = "")
        {
            IsSuccess = isSuccess;
            Message = message;
            TransactionReference = transactionReference;
        }
    }
}
