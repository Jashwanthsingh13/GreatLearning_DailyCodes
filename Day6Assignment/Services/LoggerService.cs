using System;

namespace OrderApp.Services
{
    /// <summary>
    /// LoggerService handles logging order events for audit trail
    /// </summary>
    public class LoggerService
    {
        /// <summary>
        /// Logs order placement details
        /// This method matches the OrderPlacedHandler delegate signature
        /// </summary>
        /// <param name="order">The order that was placed</param>
        public void LogOrder(Order order)
        {
            try
            {
                Console.WriteLine($"[LOGGER] Recording order in system log...");
                Console.WriteLine($"[LOGGER] Order ID: {order.OrderId}");
                Console.WriteLine($"[LOGGER] Customer: {order.CustomerName}");
                Console.WriteLine($"[LOGGER] Amount: ${order.Amount:F2}");
                Console.WriteLine($"[LOGGER] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine("[LOGGER] Order logged successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGGER] Error logging order: {ex.Message}");
            }
        }
    }
}
