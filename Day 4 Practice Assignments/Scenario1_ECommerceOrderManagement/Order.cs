using System;

namespace Scenario1_ECommerceOrderManagement
{
    /// <summary>
    /// Order class represents a customer order
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public Order(int orderId, string productName, double price, string status = "Pending")
        {
            OrderId = orderId;
            ProductName = productName;
            Price = price;
            Status = status;
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"OrderId: {OrderId}, Product: {ProductName}, Price: ${Price}, Status: {Status}, Date: {CreatedDate:dd/MM/yyyy HH:mm:ss}";
        }
    }
}
