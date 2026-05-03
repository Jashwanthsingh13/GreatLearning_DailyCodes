# FinTech Digital Wallet System (C#)

## Overview

A professionally designed Digital Wallet system built with **SOLID principles**, demonstrating clean architecture, loose coupling, and easy extensibility. This implementation serves as a reference for building scalable, maintainable financial software.

## Features

✅ **Add Money** - Load funds into wallet  
✅ **Make Payments** - Support for UPI, Card, and Net Banking  
✅ **Notifications** - Multi-channel notifications (Email, SMS, Push)  
✅ **Transaction History** - Track all wallet transactions  
✅ **Easy to Extend** - Add new payment methods without modifying existing code  
✅ **Fully Testable** - Dependency injection enables comprehensive unit testing  

## Project Structure

```
CSharp_Wallet/
├── Models/                           # Data models
│   └── Transaction.cs                # Transaction entity
├── Interfaces/                       # Contract definitions (Abstraction)
│   ├── IPaymentProcessor.cs         # Payment processing contract
│   ├── INotificationService.cs      # Notification contract
│   └── ITransactionRepository.cs    # Data persistence contract
├── Services/                         # Service implementations
│   ├── PaymentProcessors/           # Payment method implementations
│   │   └── PaymentProcessors.cs     # UPI, Card, Net Banking
│   ├── Notifications/               # Notification channel implementations
│   │   └── NotificationServices.cs  # Email, SMS, Push, Composite
│   └── Repository/                  # Data access implementations
│       └── InMemoryTransactionRepository.cs
├── Core/                            # Business logic
│   └── WalletService.cs             # Main wallet service
├── DependencyInjection/             # Service configuration
│   └── WalletServiceProvider.cs     # Build & configure services
├── Tests/                           # Unit tests
│   └── WalletServiceTests.cs        # Test suite with mocks
└── Program.cs                       # Demo application
```

## SOLID Principles Implementation

### 1. **S - Single Responsibility Principle**

Each class has one reason to change:

- **`Transaction`** → Only represents transaction data
- **`WalletService`** → Only manages wallet operations
- **`UPIPaymentProcessor`** → Only handles UPI payment logic
- **`EmailNotificationService`** → Only sends email notifications
- **`InMemoryTransactionRepository`** → Only persists transactions

```csharp
// ✅ GOOD: Focused responsibility
public class EmailNotificationService : INotificationService
{
    public async Task SendAsync(Notification notification)
    {
        // Only sends email, nothing else
    }
}

// ❌ BAD: Multiple responsibilities (if it were mixed)
// public class PaymentAndNotificationService
// {
//     public void ProcessAndNotify() { } // Too many responsibilities
// }
```

---

### 2. **O - Open/Closed Principle**

Classes are **open for extension, closed for modification**:

**Adding a new payment method is simple:**

```csharp
// 1. Create new payment processor (no modification needed)
public class GooglePayPaymentProcessor : IPaymentProcessor
{
    public string PaymentMethodName => "GOOGLEPAY";
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Implementation
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        // Validation
    }
}

// 2. Register it (no changes to WalletService)
provider.RegisterPaymentProcessor("GOOGLEPAY", new GooglePayPaymentProcessor());

// ✅ Ready to use - NO changes to existing code!
```

---

### 3. **L - Liskov Substitution Principle**

All implementations are **perfectly substitutable**:

```csharp
// These are interchangeable - each implements IPaymentProcessor
IPaymentProcessor processor1 = new UPIPaymentProcessor();
IPaymentProcessor processor2 = new CardPaymentProcessor();
IPaymentProcessor processor3 = new NetBankingPaymentProcessor();

// Same interface, consistent behavior
var result1 = await processor1.ProcessPaymentAsync(request);
var result2 = await processor2.ProcessPaymentAsync(request);
var result3 = await processor3.ProcessPaymentAsync(request);
// All return PaymentResult the same way
```

**Notification services are also substitutable:**

```csharp
INotificationService service1 = new EmailNotificationService();
INotificationService service2 = new SMSNotificationService();
INotificationService service3 = new PushNotificationService();

// All work the same way
await service1.SendAsync(notification);
await service2.SendAsync(notification);
await service3.SendAsync(notification);
```

---

### 4. **I - Interface Segregation Principle**

Clients depend only on **what they need**:

```csharp
// ✅ GOOD: Specific interfaces
public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    bool ValidatePaymentDetails(PaymentRequest request);
}

public interface INotificationService
{
    Task SendAsync(Notification notification);
}

public interface ITransactionRepository
{
    Task SaveTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
}

// ❌ BAD: Fat interface (not used in this project)
// public interface IWalletOperations
// {
//     // Too many unrelated methods
//     void Pay();
//     void AddMoney();
//     void SendNotification();
//     void SaveTransaction();
// }
```

---

### 5. **D - Dependency Inversion Principle**

High-level modules depend on **abstractions, not concretions**:

```csharp
// ✅ GOOD: Depends on interfaces (abstractions)
public class WalletService
{
    private readonly IPaymentProcessor _processor;
    private readonly INotificationService _notificationService;
    private readonly ITransactionRepository _repository;
    
    public WalletService(
        IPaymentProcessor processor,
        INotificationService notificationService,
        ITransactionRepository repository)
    {
        _processor = processor;
        _notificationService = notificationService;
        _repository = repository;
    }
}

// ❌ BAD: Direct dependency on concrete class
// public class WalletService
// {
//     private readonly UPIPaymentProcessor _processor = new();
//     private readonly EmailNotificationService _service = new();
// }
```

**Dependency Injection via Provider:**

```csharp
var provider = new WalletServiceProvider();

provider
    .RegisterPaymentProcessor("UPI", new UPIPaymentProcessor())
    .RegisterPaymentProcessor("CARD", new CardPaymentProcessor())
    .RegisterNotificationService(new EmailNotificationService());

var wallet = provider.BuildWalletService("USER001", 5000);
```

---

## Comparison: Before vs After

### ❌ **BEFORE (Tightly Coupled, Violating SOLID)**

```csharp
public class OldWalletService
{
    // Tightly coupled - hard to test, hard to extend
    private UPIProcessor _upiProcessor = new();
    private CardProcessor _cardProcessor = new();
    private EmailService _emailService = new();
    private MySQLDatabase _database = new();
    
    public void MakePayment(string method, decimal amount)
    {
        // All payment logic mixed together
        if (method == "UPI")
        {
            _upiProcessor.Process(amount); // Tightly coupled
        }
        else if (method == "CARD")
        {
            _cardProcessor.Process(amount); // Tightly coupled
        }
        
        // Direct database call
        _database.Save(transaction); // Violates DIP
    }
    
    // To add Google Pay, must modify this class (violates OCP)
    // Hard to test because of concrete dependencies
    // Can't change notification method without modifying class
}
```

**Problems:**
- ❌ Can't test without real payment processors
- ❌ Adding new payment type requires modifying class
- ❌ Hard to use different notification service
- ❌ Tightly coupled to specific database

---

### ✅ **AFTER (Loosely Coupled, SOLID Compliant)**

```csharp
public class WalletService
{
    // Loosely coupled - depends on abstractions
    private readonly Dictionary<string, IPaymentProcessor> _processors;
    private readonly INotificationService _notificationService;
    private readonly ITransactionRepository _repository;
    
    public WalletService(
        Dictionary<string, IPaymentProcessor> processors,
        INotificationService notificationService,
        ITransactionRepository repository)
    {
        _processors = processors;           // Abstraction
        _notificationService = notificationService; // Abstraction
        _repository = repository;           // Abstraction
    }
    
    public async Task MakePaymentAsync(string method, decimal amount, string recipientId)
    {
        // Generic payment processing - works with any processor
        var processor = _processors[method];
        var result = await processor.ProcessPaymentAsync(request);
        
        // Generic notification - works with any service
        await _notificationService.SendAsync(notification);
        
        // Generic persistence - works with any repository
        await _repository.SaveTransactionAsync(transaction);
    }
}
```

**Benefits:**
- ✅ Easy to test with mock implementations
- ✅ Adding new payment type requires NO modifications
- ✅ Can swap notification service anytime
- ✅ Can change database without changing wallet service
- ✅ Fully follows SOLID principles

---

## Usage Examples

### Basic Setup

```csharp
// 1. Configure payment processors
var provider = new WalletServiceProvider();

provider
    .RegisterPaymentProcessor("UPI", new UPIPaymentProcessor())
    .RegisterPaymentProcessor("CARD", new CardPaymentProcessor())
    .RegisterPaymentProcessor("NETBANKING", new NetBankingPaymentProcessor());

// 2. Configure notifications (multi-channel)
var notificationService = new CompositeNotificationService(
    new EmailNotificationService(),
    new SMSNotificationService(),
    new PushNotificationService()
);

provider.RegisterNotificationService(notificationService);

// 3. Build wallet service
var wallet = provider.BuildWalletService("USER001", 5000);
```

### Make a Payment

```csharp
// Add money to wallet
await wallet.AddMoneyAsync(1000, "Salary Credit");

// Make payment via UPI
bool success = await wallet.MakePaymentAsync(
    "merchant@upi",      // Recipient
    500,                 // Amount
    "UPI",               // Payment method
    "user@bank"          // Payment details
);

// Check balance
decimal balance = wallet.GetBalance();
```

### View Transaction History

```csharp
// Get all transactions
var transactions = await wallet.GetTransactionHistoryAsync();

// Get transactions in date range
var periodTransactions = await wallet.GetTransactionHistoryAsync(
    DateTime.Now.AddDays(-30),
    DateTime.Now
);

foreach (var txn in transactions)
{
    Console.WriteLine($"{txn.Timestamp}: {txn.Type} ₹{txn.Amount} via {txn.PaymentMethod}");
}
```

### Adding a New Payment Method

**Step 1: Create new processor**

```csharp
public class WhatsAppPayPaymentProcessor : IPaymentProcessor
{
    public string PaymentMethodName => "WHATSAPPPAY";
    
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // WhatsApp Pay implementation
        var result = await WhatsAppPayGateway.ProcessAsync(request);
        return new PaymentResult(true, "Payment processed via WhatsApp Pay", reference);
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        return request.Amount > 0 && request.PaymentDetails.Contains("wa");
    }
}
```

**Step 2: Register (no other changes needed)**

```csharp
provider.RegisterPaymentProcessor("WHATSAPPPAY", new WhatsAppPayPaymentProcessor());
```

**Step 3: Use it immediately**

```csharp
await wallet.MakePaymentAsync("recipient", 100, "WHATSAPPPAY", "+919876543210");
```

✅ **NO changes to WalletService!** This is the **Open/Closed Principle** in action.

---

## Testing

The design enables **comprehensive, isolated unit testing**:

```csharp
// Test with mock payment processor
var mockProcessor = new MockPaymentProcessor(shouldSucceed: true);
var wallet = new WalletService(
    new Dictionary<string, IPaymentProcessor> { { "MOCK", mockProcessor } },
    new MockNotificationService(),
    new InMemoryTransactionRepository()
);

// Test payment success
var result = await wallet.MakePaymentAsync("recipient", 100, "MOCK", "details");
Assert.True(result);

// Test balance update
Assert.Equal(900, wallet.GetBalance());

// Test transaction creation
var transactions = await wallet.GetTransactionHistoryAsync();
Assert.NotEmpty(transactions);
```

---

## Key Design Patterns Used

1. **Dependency Injection** - Manage dependencies elegantly
2. **Repository Pattern** - Abstract data persistence
3. **Strategy Pattern** - Different payment processors
4. **Composite Pattern** - Multiple notification channels
5. **Builder Pattern** - Configure services via WalletServiceProvider

---

## Benefits of This Architecture

| Aspect | Benefit |
|--------|---------|
| **Extensibility** | Add payment methods, notification channels without modifying existing code |
| **Testability** | Mock implementations make unit testing straightforward |
| **Maintainability** | Changes are localized; easier to maintain |
| **Reusability** | Services can be reused in different contexts |
| **Flexibility** | Swap implementations at runtime |
| **Loose Coupling** | Services don't depend on concrete implementations |

---

## Running the Application

### Prerequisites
- .NET 6.0 or higher
- C# language support

### Build & Run

```bash
dotnet build
dotnet run
```

### Run Tests

```bash
dotnet test
```

---

## Real-World Improvements

To make this production-ready:

1. **Database**: Replace `InMemoryTransactionRepository` with SQL/NoSQL implementation
2. **Authentication**: Add user authentication and authorization
3. **Encryption**: Encrypt sensitive payment details
4. **Logging**: Add comprehensive logging (Serilog)
5. **Error Handling**: Implement global exception handling
6. **Configuration**: Use appsettings.json for configuration
7. **Async**: Ensure all I/O operations are truly async
8. **Validation**: Add comprehensive input validation
9. **Rate Limiting**: Prevent abuse
10. **Audit Trail**: Log all transactions for compliance

---

## Conclusion

This implementation demonstrates how **SOLID principles** lead to:
- ✅ Clean, maintainable code
- ✅ Easy to extend without breaking existing functionality
- ✅ Simple to test with mocks
- ✅ Loose coupling between components
- ✅ Professional-grade architecture

Use this as a reference for building scalable financial software systems!
