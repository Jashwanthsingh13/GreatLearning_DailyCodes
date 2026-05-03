using System.Threading.Tasks;

namespace FinTech.Wallet.Interfaces
{
    /// <summary>
    /// Interface for sending notifications
    /// Interface Segregation: Clients depend only on what they need
    /// Single Responsibility: Only handles notification sending
    /// </summary>
    public interface INotificationService
    {
        Task SendAsync(Notification notification);
    }

    /// <summary>
    /// Notification model
    /// </summary>
    public class Notification
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }

        public Notification(string userId, string title, string message, NotificationType type)
        {
            UserId = userId;
            Title = title;
            Message = message;
            Type = type;
        }
    }

    public enum NotificationType
    {
        TransactionConfirmation,
        TransactionFailure,
        WalletLoaded,
        PaymentReminder,
        SecurityAlert
    }
}
