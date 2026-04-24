using System;

namespace Scenario1_ECommerceOrderManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display system info
            OrderManagementSystem.DisplaySystemInfo();

            // Create order management system
            OrderManagementSystem orderSystem = new OrderManagementSystem();

            // Add customers
            orderSystem.AddCustomer(new Customer(1, "John Doe", "john@example.com", "123-456-7890"));
            orderSystem.AddCustomer(new Customer(2, "Jane Smith", "jane@example.com", "987-654-3210"));
            orderSystem.AddCustomer(new Customer(3, "Bob Johnson", "bob@example.com", "555-666-7777"));

            // Add product categories
            orderSystem.AddProductCategory("Electronics");
            orderSystem.AddProductCategory("Clothing");
            orderSystem.AddProductCategory("Books");
            orderSystem.AddProductCategory("Home & Garden");
            orderSystem.AddProductCategory("Electronics"); // Duplicate - will not be added

            // Display categories
            orderSystem.DisplayProductCategories();

            // Display customers
            orderSystem.DisplayAllCustomers();

            // Add orders
            orderSystem.AddOrder(new Order(101, "Laptop", 999.99, "Pending"));
            orderSystem.AddOrder(new Order(102, "T-Shirt", 25.50, "Pending"));
            orderSystem.AddOrder(new Order(103, "Programming Book", 45.00, "Pending"));
            orderSystem.AddOrder(new Order(104, "Garden Soil", 15.99, "Pending"));

            // Display all orders
            orderSystem.DisplayAllOrders();

            // Update order status
            orderSystem.UpdateOrderStatus(101, "Processing");
            orderSystem.UpdateOrderStatus(102, "Shipped");

            // Process orders from queue (FIFO)
            orderSystem.ProcessOrders();

            // Update more statuses
            orderSystem.UpdateOrderStatus(103, "Delivered");
            orderSystem.UpdateOrderStatus(104, "Cancelled");

            // Display order status history (LIFO)
            orderSystem.DisplayStatusHistory();

            // Display final state
            orderSystem.DisplayAllOrders();

            // Summary
            Console.WriteLine("=== System Summary ===");
            Console.WriteLine($"Total Orders: {orderSystem.GetOrdersCount()}");
            Console.WriteLine($"Total Customers: {orderSystem.GetCustomersCount()}");
            Console.WriteLine($"Total Categories: 4 (unique)");
        }
    }
}
