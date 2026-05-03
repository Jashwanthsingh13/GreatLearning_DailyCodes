# SOLID Principles: Problems & Solutions

## Executive Summary

This document illustrates common SOLID principle violations in the original (poorly designed) Digital Wallet system and shows how the refactored implementation solves these problems.

---

## Problem 1: Violating Single Responsibility Principle (SRP)

### ❌ **POOR DESIGN** - Multiple Responsibilities in One Class

```csharp
// BAD: This class does TOO MUCH
public class WalletManager
{
    private decimal balance;
    private List<Transaction> transactions = new();
    
    // Payment processing - Responsibility #1
    public bool ProcessUPIPayment(decimal amount, string upiId)
    {
        // UPI-specific logic mixed in
        if (!upiId.Contains("@")) return false;
        
        // Direct API call - tightly coupled
        var gateway = new UPIGateway();
        bool success = gateway.Process(amount, upiId);
        
        if (success)
        {
            balance -= amount;
            
            // Notification logic - Responsibility #2
            EmailService.Send($"Payment of ₹{amount} processed");
            SMSService.Send($"Payment successful");
            
            // Database logic - Responsibility #3
            SaveToDatabase(new Transaction { Amount = amount, Type = "Debit" });
            
            WriteToLog($"Payment processed at {DateTime.Now}"); // Logging - Responsibility #4
        }
        
        return success;
    }
    
    public bool ProcessCardPayment(decimal amount, string cardNumber)
    {
        // Card-specific logic mixed in
        if (cardNumber.Length != 16) return false;
        
        var gateway = new CardGateway();
        bool success = gateway.Process(amount, cardNumber);
        
        if (success)
        {
            balance -= amount;
            EmailService.Send($"Card payment of ₹{amount} processed");
            SMSService.Send($"Payment successful");
            SaveToDatabase(new Transaction { Amount = amount, Type = "Debit" });
            WriteToLog($"Card payment processed at {DateTime.Now}");
        }
        
        return success;
    }
    
    public bool ProcessNetBankingPayment(decimal amount, string accountNumber)
    {
        // Net Banking logic mixed in
        if (string.IsNullOrEmpty(accountNumber)) return false;
        
        var gateway = new NetBankingGateway();
        bool success = gateway.Process(amount, accountNumber);
        
        if (success)
        {
            balance -= amount;
            EmailService.Send($"Net Banking payment of ₹{amount} processed");
            SMSService.Send($"Payment successful");
            SaveToDatabase(new Transaction { Amount = amount, Type = "Debit" });
            WriteToLog($"Net Banking payment processed at {DateTime.Now}");
        }
        
        return success;
    }
    
    // ... more payment methods to add in future means more modifications
}
```

**Problems:**
- 😢 **5 reasons to change**: Payment logic, notifications, database, logging, balance management
- 😢 **Code duplication**: Same pattern repeated for each payment type
- 😢 **Hard to test**: Can't test payment logic without triggering notifications and database saves
- 😢 **Hard to extend**: Adding new payment type means modifying this large class
- 😢 **Violates SRP**: Does payment, notification, persistence, logging

---

### ✅ **GOOD DESIGN** - Single Responsibility Each

```csharp
// ✅ GOOD: Each class has ONE reason to change

// 1. Payment processors - ONLY handle payment processing
public class UPIPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Only UPI payment logic
        var gateway = new UPIGateway();
        var result = await gateway.ProcessAsync(request.Amount, request.PaymentDetails);
        return new PaymentResult(result.Success, result.Message, result.Reference);
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        return request.PaymentDetails.Contains("@");
    }
}

// 2. Notification services - ONLY handle notifications
public class EmailNotificationService : INotificationService
{
    public async Task SendAsync(Notification notification)
    {
        // Only email sending logic
        await EmailGateway.SendEmailAsync(notification.UserId, notification.Title, notification.Message);
    }
}

// 3. Transaction repository - ONLY handles persistence
public class TransactionRepository : ITransactionRepository
{
    public async Task SaveTransactionAsync(Transaction transaction)
    {
        // Only database save logic
        await _database.SaveAsync(transaction);
    }
}

// 4. Wallet service - ONLY orchestrates wallet operations
public class WalletService
{
    public async Task MakePaymentAsync(string recipientId, decimal amount, string paymentMethod, string details)
    {
        // Delegates to specialists
        var processor = _paymentProcessors[paymentMethod];  // Use payment processor
        var result = await processor.ProcessPaymentAsync(new PaymentRequest(amount, recipientId, details, txnId));
        
        if (result.IsSuccess)
        {
            _balance -= amount;
            await _notificationService.SendAsync(new Notification(...));  // Use notification service
            await _repository.SaveTransactionAsync(new Transaction(...)); // Use repository
        }
    }
}
```

**Benefits:**
- ✅ **One reason to change each class**: Clean separation
- ✅ **No code duplication**: Generic orchestration
- ✅ **Easy to test**: Test each component independently
- ✅ **Easy to extend**: Add new payment type in new file
- ✅ **Follows SRP**: Each class has single responsibility

---

## Problem 2: Violating Open/Closed Principle (OCP)

### ❌ **POOR DESIGN** - Must Modify Existing Code to Add Features

```csharp
public class PaymentService
{
    public bool ProcessPayment(decimal amount, string paymentType, string details)
    {
        // To add each new payment type, must modify THIS METHOD
        // VIOLATES OCP: Closed for modification, but we MUST modify it!
        
        if (paymentType == "UPI")
        {
            return new UPIGateway().Process(amount, details);
        }
        else if (paymentType == "CARD")
        {
            return new CardGateway().Process(amount, details);
        }
        else if (paymentType == "NETBANKING")
        {
            return new NetBankingGateway().Process(amount, details);
        }
        // When Google Pay is added: must add another "else if" HERE → MODIFICATION!
        else if (paymentType == "GOOGLEPAY")  // ← MODIFICATION REQUIRED
        {
            return new GooglePayGateway().Process(amount, details);
        }
        // When WhatsApp Pay is added: must add yet another "else if" HERE → MORE MODIFICATIONS!
        else if (paymentType == "WHATSAPPPAY") // ← MORE MODIFICATIONS
        {
            return new WhatsAppPayGateway().Process(amount, details);
        }
        else
        {
            throw new NotSupportedException($"Payment type {paymentType} not supported");
        }
    }
}

// TEST: This code FAILS OCP because:
// - To add Google Pay → must modify ProcessPayment method
// - To add WhatsApp Pay → must modify ProcessPayment method again
// - Every new payment type → more modifications to existing code
// - Risk: Existing code might break during modifications
```

**Problems:**
- 😢 **Modification required**: Must change existing code for each new payment type
- 😢 **Risk of regression**: Modifications might break existing functionality
- 😢 **Code growth**: Method becomes longer and harder to maintain
- 😢 **Testing impact**: Must re-test entire method when adding new type
- 😢 **Violates OCP**: NOT closed for modification

---

### ✅ **GOOD DESIGN** - Open for Extension, Closed for Modification

```csharp
// ✅ GOOD: Extensible design using Strategy Pattern + Dependency Injection

// Abstract interface (closed for modification)
public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
}

// Existing implementations (closed for modification)
public class UPIPaymentProcessor : IPaymentProcessor { /*...*/ }
public class CardPaymentProcessor : IPaymentProcessor { /*...*/ }
public class NetBankingPaymentProcessor : IPaymentProcessor { /*...*/ }

// Payment service (closed for modification - never touches this again!)
public class WalletService
{
    private readonly Dictionary<string, IPaymentProcessor> _processors;
    
    public WalletService(Dictionary<string, IPaymentProcessor> processors)
    {
        _processors = processors;  // Injected dependencies
    }
    
    public async Task<bool> MakePaymentAsync(string paymentMethod, PaymentRequest request)
    {
        // ✅ NO MODIFICATION NEEDED: Works with any processor!
        var processor = _processors[paymentMethod];
        return (await processor.ProcessPaymentAsync(request)).IsSuccess;
    }
}

// ============ ADDING NEW PAYMENT METHOD ============

// Step 1: Write new processor (NO MODIFICATION to existing code)
public class GooglePayPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var gateway = new GooglePayGateway();
        var result = await gateway.ProcessAsync(request.Amount, request.PaymentDetails);
        return new PaymentResult(result.Success, result.Message, result.Reference);
    }
}

// Step 2: Register it (configuration, not modification)
var processors = new Dictionary<string, IPaymentProcessor>
{
    { "UPI", new UPIPaymentProcessor() },
    { "CARD", new CardPaymentProcessor() },
    { "NETBANKING", new NetBankingPaymentProcessor() },
    { "GOOGLEPAY", new GooglePayPaymentProcessor() }  // ← Just added, no existing code changed!
};

var wallet = new WalletService(processors);

// Step 3: Use it immediately - WalletService remains unchanged!
await wallet.MakePaymentAsync("GOOGLEPAY", request);

// ✅ SUCCESS: Added Google Pay without modifying WalletService
// - WalletService is CLOSED for modification
// - System is OPEN for extension through new processors
```

**Benefits:**
- ✅ **No modifications**: Add new payment types without changing existing code
- ✅ **No regression risk**: Existing payment methods unaffected
- ✅ **Scalable**: System grows without becoming complex
- ✅ **Reduced testing**: Only test new processor, not entire system
- ✅ **Follows OCP**: Open for extension, closed for modification

---

## Problem 3: Violating Liskov Substitution Principle (LSP)

### ❌ **POOR DESIGN** - Implementations Behave Differently

```csharp
// BAD: Different payment processors with INCOMPATIBLE behaviors

public class FastPaymentProcessor
{
    public PaymentResult ProcessPayment(decimal amount, string details)
    {
        // Returns synchronously - FAST
        return new PaymentResult { Success = true };
    }
}

public class AsyncPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string details)
    {
        // Returns asynchronously - DIFFERENT interface!
        await Task.Delay(1000);
        return new PaymentResult { Success = true };
    }
}

public class WebhookPaymentProcessor
{
    public void Process(decimal amount, string details, Action<PaymentResult> callback)
    {
        // Uses callback - COMPLETELY DIFFERENT!
        // No consistent way to use these
    }
}

// Usage: Can't substitute one for another - contractual mismatch
FastPaymentProcessor fast = new();
CreatePayment(fast.ProcessPayment(100, "")); // Synchronous call

// This won't work - different method signature:
// CreatePayment(async.ProcessPaymentAsync(100, ""));  // Different!

// This won't work - callback-based:
// CreatePayment(webhook.Process(100, "", callback));  // Different!

// VIOLATION: Can't substitute WebhookPaymentProcessor for FastPaymentProcessor
```

**Problems:**
- 😢 **Incompatible interfaces**: Different method signatures
- 😢 **Can't substitute**: Can't use different implementations interchangeably
- 😢 **Client code complexity**: Must know which implementation type it's dealing with
- 😢 **Testing nightmare**: Can't create unified mocks

---

### ✅ **GOOD DESIGN** - All Implementations Follow Same Contract

```csharp
// ✅ GOOD: Common interface ensures consistent behavior

public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    bool ValidatePaymentDetails(PaymentRequest request);
}

// All implementations follow THE SAME contract
public class UPIPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Always returns Task<PaymentResult>
        await Task.Delay(500);
        return new PaymentResult(true, "Success", reference);
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        return request.PaymentDetails.Contains("@");
    }
}

public class CardPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Same contract - returns Task<PaymentResult>
        await Task.Delay(800);
        return new PaymentResult(true, "Success", reference);
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        return request.PaymentDetails.Length >= 16;
    }
}

public class NetBankingPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Same contract - returns Task<PaymentResult>
        await Task.Delay(1000);
        return new PaymentResult(true, "Success", reference);
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        return !string.IsNullOrEmpty(request.PaymentDetails);
    }
}

// ✅ Perfect substitution - all work the same way
IPaymentProcessor processor1 = new UPIPaymentProcessor();
IPaymentProcessor processor2 = new CardPaymentProcessor();
IPaymentProcessor processor3 = new NetBankingPaymentProcessor();

// All can be used identically
var result1 = await processor1.ProcessPaymentAsync(request);
var result2 = await processor2.ProcessPaymentAsync(request);
var result3 = await processor3.ProcessPaymentAsync(request);

// Can swap at runtime - same behavior expected
public async Task ProcessWithAnyProcessor(IPaymentProcessor processor)
{
    var result = await processor.ProcessPaymentAsync(request);
    if (result.IsSuccess)
    {
        console.WriteLine("Payment successful");
    }
}

// This works with ANY implementation:
await ProcessWithAnyProcessor(new UPIPaymentProcessor());     // ✅ Works
await ProcessWithAnyProcessor(new CardPaymentProcessor());    // ✅ Works
await ProcessWithAnyProcessor(new NetBankingPaymentProcessor()); // ✅ Works
```

**Benefits:**
- ✅ **Consistent contract**: All implementations behave predictably
- ✅ **Perfect substitution**: Can swap implementations at runtime
- ✅ **Polymorphism works**: Clients don't care which implementation
- ✅ **Easy testing**: Create compatible mocks

---

## Problem 4: Violating Interface Segregation Principle (ISP)

### ❌ **POOR DESIGN** - Fat Interfaces Force Unnecessary Implementation

```csharp
// BAD: One enormous interface that forces implementations
// to provide methods they don't need

public interface IPaymentService
{
    // Payment methods - needed by all
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    
    // Notification methods - not all need these!
    Task SendEmailAsync(string email, string subject, string body);
    Task SendSMSAsync(string phone, string message);
    Task SendPushNotificationAsync(string userId, string message);
    
    // Reporting methods - not all need these!
    Task<List<TransactionReport>> GetTransactionReportAsync(DateTime from, DateTime to);
    Task<List<UserReport>> GetUserReportAsync();
    
    // Refund methods - not all need these!
    Task<bool> ProcessRefundAsync(string transactionId);
    Task<bool> ProcessPartialRefundAsync(string transactionId, decimal amount);
    
    // Dispute methods - not all need these!
    Task<bool> FilaDisputeAsync(string transactionId, string reason);
    Task<DisputeStatus> GetDisputeStatusAsync(string transactionId);
}

// Forced to implement ALL methods even if not needed!
public class UPIPaymentProcessor : IPaymentService
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        // Actually uses this
        return await new UPIGateway().ProcessAsync(request);
    }
    
    // Forced to implement these - WASTES CODE
    public async Task SendEmailAsync(string email, string subject, string body)
    {
        throw new NotImplementedException(); // Not needed!
    }
    
    public async Task SendSMSAsync(string phone, string message)
    {
        throw new NotImplementedException(); // Not needed!
    }
    
    public async Task SendPushNotificationAsync(string userId, string message)
    {
        throw new NotImplementedException(); // Not needed!
    }
    
    // ... more unnecessary implementations ...
}

// Can't use payment processor JUST for payments because interface includes reporting, disputes, etc.
// Clients must reference entire fat interface
public class PaymentController
{
    public async Task<ActionResult> MakePayment(IPaymentService service)
    {
        // Has access to dispute, refund, reporting methods even though
        // this controller only needs ProcessPaymentAsync()
        var result = await service.ProcessPaymentAsync(request);
    }
}
```

**Problems:**
- 😢 **Unnecessary implementations**: Must implement methods that don't apply
- 😢 **Code bloat**: NotImplementedExceptions() everywhere
- 😢 **Confusing contracts**: Client sees methods it can't use
- 😢 **Rigid coupling**: Hard to change interface without affecting everyone

---

### ✅ **GOOD DESIGN** - Segregated, Focused Interfaces

```csharp
// ✅ GOOD: Separate, focused interfaces
// Clients depend ONLY on what they need

// Interface #1: Payment processing only
public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    bool ValidatePaymentDetails(PaymentRequest request);
}

// Interface #2: Notifications only
public interface INotificationService
{
    Task SendAsync(Notification notification);
}

// Interface #3: Transaction history only
public interface ITransactionRepository
{
    Task SaveTransactionAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
}

// Interface #4: Reporting only (if needed separately)
public interface IReportingService
{
    Task<List<TransactionReport>> GenerateTransactionReportAsync(DateTime from, DateTime to);
    Task<List<UserReport>> GenerateUserReportAsync();
}

// Interface #5: Dispute handling only (if needed separately)
public interface IDisputeService
{
    Task<bool> FileDisputeAsync(string transactionId, string reason);
    Task<DisputeStatus> GetDisputeStatusAsync(string transactionId);
}

// Now implementations are CLEAN - only implement what they need
public class UPIPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        return await new UPIGateway().ProcessAsync(request);
    }
    
    public bool ValidatePaymentDetails(PaymentRequest request)
    {
        return request.PaymentDetails.Contains("@");
    }
    
    // No unnecessary methods! Clean and focused.
}

public class EmailNotificationService : INotificationService
{
    public async Task SendAsync(Notification notification)
    {
        await EmailGateway.SendEmailAsync(notification.UserId, notification.Title, notification.Message);
    }
    
    // Only what's needed! Clean and focused.
}

// Clients depend only on what they use
public class WalletService
{
    // Only needs these two interfaces
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly INotificationService _notificationService;
    
    public WalletService(
        IPaymentProcessor paymentProcessor,
        INotificationService notificationService)
    {
        _paymentProcessor = paymentProcessor;      // Only payment logic
        _notificationService = notificationService; // Only notification logic
    }
}

public class ReportingService
{
    // Only needs this interface
    private readonly IReportingService _reportingService;
    
    public ReportingService(IReportingService reportingService)
    {
        _reportingService = reportingService; // Only reporting logic
    }
}
```

**Benefits:**
- ✅ **Clean implementations**: No unnecessary methods
- ✅ **Focused dependencies**: Clients use only what they need
- ✅ **Easy to understand**: Interface shows exactly what it does
- ✅ **Flexible**: Can combine interfaces without waste
- ✅ **Follows ISP**: Small, focused interfaces

---

## Problem 5: Violating Dependency Inversion Principle (DIP)

### ❌ **POOR DESIGN** - Direct Dependency on Concrete Classes

```csharp
// BAD: High-level module depends on low-level details
// VIOLATES DIP: Should depend on abstractions, not concretions

public class WalletService
{
    // Directly creating concrete instances - TIGHTLY COUPLED!
    private UPIGateway _upiGateway = new();           // Concrete class dependency
    private CardGateway _cardGateway = new();         // Concrete class dependency
    private EmailService _emailService = new();       // Concrete class dependency
    private MySQLDatabase _database = new();          // Concrete class dependency
    private FileLogger _logger = new();               // Concrete class dependency
    
    public bool ProcessUPIPayment(decimal amount, string upiId)
    {
        // Directly using concrete gateway
        var result = _upiGateway.Process(amount, upiId);
        
        if (result.Success)
        {
            // Directly using concrete email service
            _emailService.SendEmail("user@example.com", $"Payment of ₹{amount} confirmed");
            
            // Directly using concrete database
            _database.Save(new TransactionRecord { Amount = amount, Type = "Debit" });
            
            // Directly using concrete file logger
            _logger.Log($"Payment of ₹{amount} processed at {DateTime.Now}");
        }
        
        return result.Success;
    }
}

// PROBLEMS WITH THIS APPROACH:
// 1. Can't test with mock objects - new() creates real instances
//    var wallet = new WalletService(); // Creates REAL database connection!
//
// 2. Changing implementation requires modifying WalletService
//    Want to switch to PostgreSQL? Must change WalletService!
//    Want to use SendGrid for email? Must change WalletService!
//
// 3. Dependencies are in constructor or class level
//    Can't inject different behavior for testing
//
// 4. Concrete classes might fail
//    Database connection fails → entire WalletService fails!
//
// 5. High coupling - many reasons to change WalletService
//    When UPIGateway changes → update WalletService
//    When EmailService changes → update WalletService
//    When MySQLDatabase changes → update WalletService
```

**Problems:**
- 😢 **Untestable**: Can't mock dependencies
- 😢 **Tightly coupled**: Changes to gateways/services require modifying WalletService
- 😢 **Hard to adapt**: Switching implementations is difficult
- 😢 **Fragile**: Concrete class failures cascade

---

### ✅ **GOOD DESIGN** - Depend on Abstractions

```csharp
// ✅ GOOD: High-level module depends on abstractions
// FOLLOWS DIP: Depends on interfaces, not concrete classes

// Define abstractions
public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
}

public interface INotificationService
{
    Task SendAsync(Notification notification);
}

public interface ITransactionRepository
{
    Task SaveTransactionAsync(Transaction transaction);
}

public interface ILogger
{
    void Log(string message);
}

// High-level module depends on abstractions
public class WalletService
{
    // Dependencies are INJECTED abstractions, not concrete classes
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly INotificationService _notificationService;
    private readonly ITransactionRepository _repository;
    private readonly ILogger _logger;
    
    // Constructor Injection - receives abstractions, not concrete classes
    public WalletService(
        IPaymentProcessor paymentProcessor,
        INotificationService notificationService,
        ITransactionRepository repository,
        ILogger logger)
    {
        _paymentProcessor = paymentProcessor;          // Abstraction
        _notificationService = notificationService;    // Abstraction
        _repository = repository;                      // Abstraction
        _logger = logger;                              // Abstraction
    }
    
    public async Task<bool> MakePaymentAsync(PaymentRequest request)
    {
        // Uses abstractions - implementation doesn't matter
        var result = await _paymentProcessor.ProcessPaymentAsync(request);
        
        if (result.IsSuccess)
        {
            // Uses abstraction - any notification service works
            await _notificationService.SendAsync(new Notification(...));
            
            // Uses abstraction - any repository works
            await _repository.SaveTransactionAsync(new Transaction(...));
            
            // Uses abstraction - any logger works
            _logger.Log($"Payment processed: {request.TransactionId}");
        }
        
        return result.IsSuccess;
    }
}

// ===== TESTING: Easy with mock implementations =====

// Create mock implementations (no concrete classes needed)
public class MockPaymentProcessor : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        return new PaymentResult(true, "Mock success", "REF123");
    }
}

public class MockNotificationService : INotificationService
{
    public async Task SendAsync(Notification notification)
    {
        // Do nothing - it's a mock
    }
}

public class MockRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();
    
    public async Task SaveTransactionAsync(Transaction transaction)
    {
        _transactions.Add(transaction);
    }
}

public class MockLogger : ILogger
{
    public void Log(string message)
    {
        // Do nothing - it's a mock
    }
}

// ===== IN UNIT TEST =====
[Fact]
public async Task ProcessPayment_CreatesTransaction()
{
    // Arrange - inject mock implementations
    var mockProcessor = new MockPaymentProcessor();
    var mockNotification = new MockNotificationService();
    var mockRepository = new MockRepository();
    var mockLogger = new MockLogger();
    
    var wallet = new WalletService(
        mockProcessor,        // Mock, not real
        mockNotification,     // Mock, not real
        mockRepository,       // Mock, not real
        mockLogger            // Mock, not real
    );
    
    // Act
    bool result = await wallet.MakePaymentAsync(request);
    
    // Assert
    Assert.True(result);
    // Test verifies behavior without touching database, email, etc.
}

// ===== IN PRODUCTION =====

// Create real implementations
var processor = new UPIPaymentProcessor();
var notification = new EmailNotificationService();
var repository = new SQLServerTransactionRepository();
var logger = new FileLogger();

// Inject them
var wallet = new WalletService(
    processor,      // Real UPI processor
    notification,   // Real email service
    repository,     // Real database
    logger          // Real file logger
);

// Can switch implementations without changing WalletService!
// Want to use PostgreSQL? Create new PostgreSQLTransactionRepository, inject it
// Want to use SMS notifications? Create SMSNotificationService, inject it
// WalletService never changes!
```

**Benefits:**
- ✅ **Testable**: Mock implementations enable easy testing
- ✅ **Loosely coupled**: Change implementations freely
- ✅ **Flexible**: Switch implementations based on environment/needs
- ✅ **Resilient**: Failures isolated to specific implementation
- ✅ **Maintainable**: Fewer reasons to modify high-level code
- ✅ **Follows DIP**: Depends on abstractions

---

## Summary Comparison Table

| Problem | POOR Design | GOOD Design |
|---------|------------|------------|
| **SRP** | Multiple responsibilities in one class | Each class has single responsibility |
| **OCP** | Must modify existing code to add features | Extend through new implementations |
| **LSP** | Implementations have different contracts | All follow same contract |
| **ISP** | Fat interfaces with unnecessary methods | Small, focused interfaces |
| **DIP** | Depend on concrete classes | Depend on abstractions |
| **Testability** | Hard - can't mock concrete classes | Easy - inject mocks |
| **Extensibility** | Hard - requires modification | Easy - add new implementations |
| **Coupling** | Tight - high interdependencies | Loose - minimal dependencies |
| **Maintenance** | Hard - changes ripple everywhere | Easy - changes are localized |

---

## Conclusion

By following SOLID principles:

✅ Code is cleaner and easier to understand  
✅ Features are easier to add without breaking existing code  
✅ Testing becomes straightforward with dependency injection  
✅ Changes are localized, reducing risk of bugs  
✅ System is more flexible and maintainable long-term  

**The investment in following SOLID pays dividends as the system grows!**
