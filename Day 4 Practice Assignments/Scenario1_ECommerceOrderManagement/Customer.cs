using System;

namespace Scenario1_ECommerceOrderManagement
{
    /// <summary>
    /// Customer class represents a customer in the e-commerce system
    /// </summary>
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Customer(int customerId, string name, string email, string phoneNumber)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"CustomerId: {CustomerId}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}";
        }
    }
}
