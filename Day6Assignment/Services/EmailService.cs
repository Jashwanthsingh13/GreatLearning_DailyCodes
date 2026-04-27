using System;

namespace OrderApp.Services
{
    /// <summary>
    /// EmailService handles sending email notifications when an order is placed
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Sends an email notification for the placed order
        /// This method matches the OrderPlacedHandler delegate signature
        /// </summary>
        /// <param name="order">The order that was placed</param>
        public void SendEmail(Order order)
        {
            try
            {
                Console.WriteLine($"[EMAIL] Sending confirmation email to customer: {order.CustomerName}");
                Console.WriteLine($"[EMAIL] Subject: Order Confirmation #{order.OrderId}");
                Console.WriteLine($"[EMAIL] Order Amount: ${order.Amount:F2}");
                Console.WriteLine("[EMAIL] Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL] Error sending email: {ex.Message}");
            }
        }
    }
}
