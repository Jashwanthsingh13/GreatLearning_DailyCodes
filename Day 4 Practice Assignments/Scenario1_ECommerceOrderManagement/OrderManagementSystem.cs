using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario1_ECommerceOrderManagement
{
    /// <summary>
    /// Interface for order operations
    /// </summary>
    public interface IOrderManagement
    {
        void AddOrder(Order order);
        void RemoveOrder(int orderId);
        void UpdateOrderStatus(int orderId, string newStatus);
        Order GetOrder(int orderId);
        void DisplayAllOrders();
    }

    /// <summary>
    /// OrderManagementSystem class implements IOrderManagement
    /// Uses various collection types as per requirements
    /// </summary>
    public class OrderManagementSystem : IOrderManagement
    {
        // Constants
        private const string SYSTEM_NAME = "E-Commerce Order Management System";
        private const int MAX_ORDERS = 1000;

        // Collections
        private List<Order> orders;                              // Store all orders
        private Dictionary<int, Customer> customers;             // Map customer ID to customer details
        private HashSet<string> productCategories;              // Store unique product categories
        private Queue<Order> orderProcessingQueue;              // FIFO order processing
        private Stack<string> orderStatusHistory;               // LIFO order status history

        public OrderManagementSystem()
        {
            orders = new List<Order>();
            customers = new Dictionary<int, Customer>();
            productCategories = new HashSet<string>();
            orderProcessingQueue = new Queue<Order>();
            orderStatusHistory = new Stack<string>();
        }

        // Static method to display system info
        public static void DisplaySystemInfo()
        {
            Console.WriteLine($"=== {SYSTEM_NAME} ===");
            Console.WriteLine($"Max Orders Capacity: {MAX_ORDERS}");
            Console.WriteLine();
        }

        /// <summary>
        /// Add a new customer to the system
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            if (!customers.ContainsKey(customer.CustomerId))
            {
                customers.Add(customer.CustomerId, customer);
                Console.WriteLine($"✓ Customer added: {customer.Name}");
            }
            else
            {
                Console.WriteLine($"✗ Customer ID {customer.CustomerId} already exists");
            }
        }

        /// <summary>
        /// Add a new order to the system
        /// </summary>
        public void AddOrder(Order order)
        {
            if (orders.Count < MAX_ORDERS)
            {
                orders.Add(order);
                orderProcessingQueue.Enqueue(order);
                orderStatusHistory.Push($"Order {order.OrderId} created with status: {order.Status}");
                Console.WriteLine($"✓ Order added: {order.OrderId} - {order.ProductName}");
            }
            else
            {
                Console.WriteLine($"✗ Cannot add order. Maximum capacity ({MAX_ORDERS}) reached.");
            }
        }

        /// <summary>
        /// Add product category to the system
        /// </summary>
        public void AddProductCategory(string category)
        {
            if (productCategories.Add(category))
            {
                Console.WriteLine($"✓ Category added: {category}");
            }
            else
            {
                Console.WriteLine($"✗ Category '{category}' already exists");
            }
        }

        /// <summary>
        /// Remove an order from the system
        /// </summary>
        public void RemoveOrder(int orderId)
        {
            Order orderToRemove = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (orderToRemove != null)
            {
                orders.Remove(orderToRemove);
                orderStatusHistory.Push($"Order {orderId} removed");
                Console.WriteLine($"✓ Order {orderId} removed successfully");
            }
            else
            {
                Console.WriteLine($"✗ Order {orderId} not found");
            }
        }

        /// <summary>
        /// Update order status
        /// </summary>
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            Order order = GetOrder(orderId);
            if (order != null)
            {
                string oldStatus = order.Status;
                order.Status = newStatus;
                orderStatusHistory.Push($"Order {orderId} status changed from '{oldStatus}' to '{newStatus}'");
                Console.WriteLine($"✓ Order {orderId} status updated to: {newStatus}");
            }
            else
            {
                Console.WriteLine($"✗ Order {orderId} not found");
            }
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        public Order GetOrder(int orderId)
        {
            return orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Process orders from queue (FIFO)
        /// </summary>
        public void ProcessOrders()
        {
            Console.WriteLine("\n--- Processing Orders (FIFO) ---");
            int processedCount = 0;
            while (orderProcessingQueue.Count > 0)
            {
                Order order = orderProcessingQueue.Dequeue();
                Console.WriteLine($"Processing: {order}");
                processedCount++;
            }
            Console.WriteLine($"Total orders processed: {processedCount}\n");
        }

        /// <summary>
        /// Display order status history (LIFO)
        /// </summary>
        public void DisplayStatusHistory()
        {
            Console.WriteLine("\n--- Order Status History (LIFO) ---");
            if (orderStatusHistory.Count == 0)
            {
                Console.WriteLine("No history available");
                return;
            }

            int count = 1;
            while (orderStatusHistory.Count > 0)
            {
                Console.WriteLine($"{count}. {orderStatusHistory.Pop()}");
                count++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all orders
        /// </summary>
        public void DisplayAllOrders()
        {
            Console.WriteLine("\n--- All Orders ---");
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all customers
        /// </summary>
        public void DisplayAllCustomers()
        {
            Console.WriteLine("\n--- All Customers ---");
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found");
                return;
            }

            foreach (var customer in customers.Values)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Display all product categories
        /// </summary>
        public void DisplayProductCategories()
        {
            Console.WriteLine("\n--- Product Categories ---");
            if (productCategories.Count == 0)
            {
                Console.WriteLine("No categories available");
                return;
            }

            var sortedCategories = productCategories.OrderBy(c => c);
            foreach (var category in sortedCategories)
            {
                Console.WriteLine($"• {category}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Get total orders count
        /// </summary>
        public int GetOrdersCount() => orders.Count;

        /// <summary>
        /// Get total customers count
        /// </summary>
        public int GetCustomersCount() => customers.Count;
    }
}
