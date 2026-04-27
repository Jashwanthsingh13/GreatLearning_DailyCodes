using System;

namespace OrderApp.Services
{
    /// <summary>
    /// SMSService handles sending SMS notifications when an order is placed
    /// </summary>
    public class SMSService
    {
        /// <summary>
        /// Sends an SMS notification for the placed order
        /// This method matches the OrderPlacedHandler delegate signature
        /// </summary>
        /// <param name="order">The order that was placed</param>
        public void SendSMS(Order order)
        {
            try
            {
                Console.WriteLine($"[SMS] Sending SMS to customer: {order.CustomerName}");
                Console.WriteLine($"[SMS] Message: Your order #{order.OrderId} has been placed for ${order.Amount:F2}");
                Console.WriteLine("[SMS] SMS sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SMS] Error sending SMS: {ex.Message}");
            }
        }
    }
}
