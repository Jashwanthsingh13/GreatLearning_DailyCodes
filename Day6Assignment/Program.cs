using System;
using OrderApp.Services;

/// <summary>
/// Real-Time Order Notification System
/// Demonstrates the use of delegates, events, and multicast delegates
/// in a publisher-subscriber pattern for loose coupling
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Real-Time Order Notification System");
        Console.WriteLine("========================================\n");

        // Initialize the order processor (Publisher)
        OrderProcessor processor = new OrderProcessor();

        // Initialize service instances (Subscribers)
        EmailService emailService = new EmailService();
        SMSService smsService = new SMSService();
        LoggerService loggerService = new LoggerService();

        Console.WriteLine("--- Subscribing services to OnOrderPlaced event ---\n");

        // Subscribe services to the OnOrderPlaced event
        // This demonstrates multicast delegate - multiple handlers attached to one event
        processor.OnOrderPlaced += emailService.SendEmail;
        processor.OnOrderPlaced += smsService.SendSMS;
        processor.OnOrderPlaced += loggerService.LogOrder;

        Console.WriteLine($"Total subscribers: {processor.GetSubscriberCount()}\n");

        // Scenario 1: Process a standard order with all subscribers
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║    SCENARIO 1: Order with All Services║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        
        Order order1 = new Order(101, "John Doe", 299.99);
        processor.ProcessOrder(order1);

        // Scenario 2: Demonstrate unsubscribe functionality
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║ SCENARIO 2: Unsubscribing SMS Service  ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        
        Console.WriteLine("\n--- Unsubscribing SMS service ---\n");
        processor.Unsubscribe(smsService.SendSMS);
        Console.WriteLine($"Total subscribers: {processor.GetSubscriberCount()}\n");

        Order order2 = new Order(102, "Jane Smith", 149.50);
        processor.ProcessOrder(order2);

        // Scenario 3: Resubscribe SMS and process another order
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  SCENARIO 3: Resubscribing SMS Service  ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        
        Console.WriteLine("\n--- Resubscribing SMS service ---\n");
        processor.OnOrderPlaced += smsService.SendSMS;
        Console.WriteLine($"Total subscribers: {processor.GetSubscriberCount()}\n");

        Order order3 = new Order(103, "Bob Johnson", 75.25);
        processor.ProcessOrder(order3);

        // Scenario 4: Demonstrate dynamic service management
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║ SCENARIO 4: Logger Only (Email & SMS)  ║");
        Console.WriteLine("║            Unsubscribed                 ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        
        Console.WriteLine("\n--- Unsubscribing Email and SMS services ---\n");
        processor.Unsubscribe(emailService.SendEmail);
        processor.Unsubscribe(smsService.SendSMS);
        Console.WriteLine($"Total subscribers: {processor.GetSubscriberCount()}\n");

        Order order4 = new Order(104, "Alice Brown", 199.99);
        processor.ProcessOrder(order4);

        Console.WriteLine("========================================");
        Console.WriteLine("Order Processing Complete");
        Console.WriteLine("========================================\n");

        // Key Learning Points
        DisplayLearningPoints();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    /// <summary>
    /// Displays key learning points about the implementation
    /// </summary>
    static void DisplayLearningPoints()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║           KEY LEARNING POINTS                          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("[1] Delegates:");
        Console.WriteLine("    - OrderPlacedHandler is a delegate type");
        Console.WriteLine("    - Defines the signature for event handlers");
        Console.WriteLine();
        Console.WriteLine("[2] Multicast Delegates:");
        Console.WriteLine("    - Multiple subscribers attached to OnOrderPlaced event");
        Console.WriteLine("    - All handlers executed when event is raised");
        Console.WriteLine();
        Console.WriteLine("[3] Event-Driven Architecture:");
        Console.WriteLine("    - OrderProcessor (Publisher) doesn't know about services");
        Console.WriteLine("    - Services (Subscribers) react to order events");
        Console.WriteLine();
        Console.WriteLine("[4] Loose Coupling:");
        Console.WriteLine("    - Services can be added/removed dynamically");
        Console.WriteLine("    - No direct dependencies between publisher and subscribers");
        Console.WriteLine();
        Console.WriteLine("[5] Null-Safe Invocation:");
        Console.WriteLine("    - Using ?. operator prevents null reference exceptions");
        Console.WriteLine("    - Event can be safely invoked even with no subscribers");
        Console.WriteLine();
    }
}
