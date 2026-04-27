/// <summary>
/// Represents an order in the e-commerce system
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier for the order
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Name of the customer who placed the order
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Total amount for the order
    /// </summary>
    public double Amount { get; set; }

    /// <summary>
    /// Constructor to initialize an order
    /// </summary>
    public Order(int orderId, string customerName, double amount)
    {
        OrderId = orderId;
        CustomerName = customerName;
        Amount = amount;
    }

    /// <summary>
    /// String representation of the order
    /// </summary>
    public override string ToString()
    {
        return $"Order ID: {OrderId}, Customer: {CustomerName}, Amount: ${Amount:F2}";
    }
}
