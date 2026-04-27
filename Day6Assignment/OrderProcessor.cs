using System;

/// <summary>
/// Delegate definition for order notification handlers
/// Represents the signature that all event subscribers must implement
/// </summary>
public delegate void OrderPlacedHandler(Order order);

/// <summary>
/// OrderProcessor class handles order processing and event publishing
/// Uses the publisher-subscriber model for loose coupling
/// </summary>
public class OrderProcessor
{
    /// <summary>
    /// Event declaration using the OrderPlacedHandler delegate
    /// This event is raised when an order is placed
    /// </summary>
    public event OrderPlacedHandler OnOrderPlaced;

    /// <summary>
    /// Processes an order and triggers notifications
    /// </summary>
    /// <param name="order">The order to process</param>
    public void ProcessOrder(Order order)
    {
        try
        {
            Console.WriteLine($"\n--- Processing {order} ---");

            // Invoke the event and notify all subscribers
            // Using null-safe invocation operator (?.Invoke) to avoid null reference exceptions
            OnOrderPlaced?.Invoke(order);

            Console.WriteLine("--- Order processing completed ---\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing order: {ex.Message}");
        }
    }

    /// <summary>
    /// Subscribe a handler to the OnOrderPlaced event
    /// </summary>
    /// <param name="handler">The handler to subscribe</param>
    public void Subscribe(OrderPlacedHandler handler)
    {
        OnOrderPlaced += handler;
    }

    /// <summary>
    /// Unsubscribe a handler from the OnOrderPlaced event
    /// </summary>
    /// <param name="handler">The handler to unsubscribe</param>
    public void Unsubscribe(OrderPlacedHandler handler)
    {
        OnOrderPlaced -= handler;
    }

    /// <summary>
    /// Get the count of current subscribers
    /// </summary>
    /// <returns>Number of active subscribers</returns>
    public int GetSubscriberCount()
    {
        if (OnOrderPlaced == null) return 0;
        return OnOrderPlaced.GetInvocationList().Length;
    }
}
