# Architecture & Design Patterns

## System Architecture

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         Digital Wallet System (C#)                          │
│                       Following SOLID Principles                            │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                         Application Layer (Main)                            │
│                    ┌──────────────────────────────────┐                    │
│                    │      WalletServiceProvider       │                    │
│                    │ (Builder + Dependency Injection) │                    │
│                    └──────────┬───────────────────────┘                    │
│                               │                                             │
│                               ▼                                             │
│                        ┌─────────────────┐                                 │
│                        │  WalletService  │                                 │
│                        │  (Orchestrator) │                                 │
│                        └────────┬────────┘                                 │
│                                 │                                           │
└─────────────────┬───────────────┼───────────────┬──────────────────────────┘
                  │               │               │
            ┌─────▼──────┐   ┌────▼────────┐  ┌──▼─────────────────┐
            │ Processors │   │Connection   │  │  Notifications     │
            └─────┬──────┘   │    Layer    │  └──┬─────────────────┘
                  │          └────┬────────┘     │
      ┌───────────┼────────┐      │              │
      │           │        │      │         ┌────┼────────┐
      ▼           ▼        ▼      ▼         │    │        │
   ┌────┐   ┌────┐   ┌────┐   ┌────┐   ┌──┴─┐┌─┴──┐  ┌──┴──┐
   │UPI │   │Card│   │NetB│   │Repo│   │Eml ││SMS │  │Push │
   └────┘   └────┘   └────┘   └────┘   └────┘└────┘  └─────┘
```

## Component Relationships

```
                          ┌─────────────────────┐
                          │  WalletService      │
                          │  (High-Level)       │
                          └──────────┬──────────┘
                                     │
                ┌────────────────────┼────────────────────┐
                │                    │                    │
                ▼                    ▼                    ▼
        ┌──────────────────┐  ┌──────────────┐  ┌──────────────────┐
        │ IPaymentProcessor│  │INotification │  │ITransactionRepos │
        │ (Abstraction)    │  │Service(Abst.)│  │itory(Abstraction)│
        └──────────┬───────┘  └──────┬───────┘  └────────┬──────────┘
                   │                 │                    │
      ┌────────────┼────────┐    ┌───┼───┐        ┌──────▼──────┐
      │            │        │    │   │   │        │             │
      ▼            ▼        ▼    ▼   ▼   ▼        ▼             ▼
   UPI        Card      NetBk Email SMS Push  InMemory     (Future: DB)
  Proc       Proc       Proc  Serv Serv Serv Repo       
(Concrete)  (Concrete) (Concrete) (Concrete implementations)
```

## Design Patterns Used

### 1. **Strategy Pattern**
Multiple payment processors implement the same interface, allowing runtime selection.

```
┌──────────────────────────────────────────────────────────┐
│                   IPaymentProcessor                      │
│  ┌──────────────────────────────────────────────────────┐│
│  │ - ProcessPaymentAsync(request): PaymentResult       ││
│  │ - ValidatePaymentDetails(request): bool             ││
│  └──────────────────────────────────────────────────────┘│
└──────────────────────────────────────────────────────────┘
        ▲                    ▲                    ▲
        │                    │                    │
        │                    │                    │
   ┌────┴─────┐         ┌────┴─────┐        ┌────┴────────┐
   │UPI Proc  │         │Card Proc │        │NetBank Proc │
   │Strategy  │         │Strategy  │        │Strategy     │
   └──────────┘         └──────────┘        └─────────────┘
```

### 2. **Dependency Injection Pattern**
Dependencies are injected through constructors, enabling loose coupling and testability.

```
WalletService Constructor:
┌─────────────────────────────────────────────────────────┐
│ public WalletService(                                   │
│     Dictionary<string, IPaymentProcessor> processors,   │
│     INotificationService notificationService,           │
│     ITransactionRepository transactionRepository)      │
│ {                                                       │
│     _processors = processors;          // Injected     │
│     _notificationService = nt;         // Injected     │
│     _repository = repository;          // Injected     │
│ }                                                       │
└─────────────────────────────────────────────────────────┘
```

### 3. **Repository Pattern**
Data access is abstracted behind ITransactionRepository interface.

```
┌──────────────────────────────────────────┐
│      ITransactionRepository              │
│  (Abstract Data Access Layer)            │
├──────────────────────────────────────────┤
│ + SaveTransactionAsync()                 │
│ + GetTransactionsByUserIdAsync()        │
│ + GetTransactionsByDateRangeAsync()    │
│ + GetTransactionByIdAsync()             │
└──────────────────────────────────────────┘
        ▲
        │
        ├─────────────────────────────────────┐
        │                                     │
   ┌────┴────────┐                  ┌────────┴─────┐
   │InMemory Repo│                  │Database Repo  │
   │(Testing)    │                  │(Production)   │
   └─────────────┘                  └───────────────┘
```

### 4. **Builder Pattern**
WalletServiceProvider provides fluent configuration for building WalletService.

```
Configuration Chain:
new WalletServiceProvider()
    .RegisterPaymentProcessor("UPI", new UPIPaymentProcessor())
    .RegisterPaymentProcessor("CARD", new CardPaymentProcessor())
    .RegisterNotificationService(new EmailNotificationService())
    .BuildWalletService("USER001", 5000);
    
Each method returns the provider for method chaining:
┌─────────────────────────────┐
│ RegisterPaymentProcessor()  │
└────────────────┬────────────┘
                 │ returns 'this'
                 ▼
┌─────────────────────────────┐
│ RegisterNotificationService()
└────────────────┬────────────┘
                 │ returns 'this'
                 ▼
┌─────────────────────────────┐
│ BuildWalletService()        │
└─────────────────────────────┘
```

### 5. **Composite Pattern (Notifications)**
Multiple notification services can be combined into one.

```
┌──────────────────────────────────────────────┐
│        CompositeNotificationService          │
│                                              │
│  List<INotificationService> _services        │
│                                              │
│  SendAsync(notification):                    │
│    - For each service in _services:          │
│      - Call service.SendAsync(notification)  │
└──────────────────────────────────────────────┘
       ▲
       │ contains
       │
 ┌─────┴──────┬────────┬────────┐
 │            │        │        │
 ▼            ▼        ▼        ▼
Email       SMS      Push     [Future]
Service     Service  Service   Services
```

## Dependency Flow (Dependency Inversion Principle)

```
┌────────────────────────────────────────────────────────┐
│           High-Level Module                           │
│           WalletService                               │
│                                                        │
│  Makes payments, manages balance, sends notifications │
└─────────────────┬──────────────────────────────────────┘
                  │ depends on
                  │ (abstractions)
        ┌─────────┼──────────┬─────────────┐
        │         │          │             │
        ▼         ▼          ▼             ▼
    ┌─────────────────────────────────────────────────┐
    │ Abstraction Layer (Interfaces)                  │
    ├─────────────────────────────────────────────────┤
    │ - IPaymentProcessor                            │
    │ - INotificationService                         │
    │ - ITransactionRepository                       │
    └─────────────────────────────────────────────────┘
        ▲         ▲          ▲             ▲
        │         │          │             │
        │ implemented by
        │ (concrete classes)
        │         │          │             │
    ┌───┴──┐  ┌──┴──┐   ┌───┴──┐   ┌─────┴────┐
    │ UPI  │  │Card │   │Email │   │InMemory  │
    │Proc  │  │Proc │   │Serv  │   │Repo      │
    │(Real)│  │(Real)   │(Real)│   │(Real)    │
    └──────┘  └──────┘   └──────┘   └──────────┘

  Low-Level Modules (Implementation Details)
```

This follows **Dependency Inversion Principle**:
- High-level modules don't depend on low-level modules
- Both depend on abstractions (interfaces)
- Easy to swap implementations

## Data Flow: Making a Payment

```
User Request
    │
    ▼
┌─────────────────────────────────────────┐
│  WalletService.MakePaymentAsync()       │
│  - Validate amount & payment method     │
│  - Check balance                        │
└────────────┬────────────────────────────┘
             │
             ▼
     ┌──────────────────────┐
     │ Select Processor     │
     │ from dictionary      │
     │ by method           │
     └──────┬───────────────┘
            │ requests
            ▼
┌─────────────────────────────────────────┐
│  IPaymentProcessor                      │
│  .ProcessPaymentAsync()                 │
│ (UPI/Card/NetBanking)                  │
└────────┬────────────────────────────────┘
         │ processes payment
         ▼
    ┌──────────────┐
    │ PaymentGway  │
    │ API Call     │
    └────┬─────────┘
         │ returns PaymentResult
         ▼
┌─────────────────────────────────────────┐
│  WalletService                          │
│ - Update balance                        │
│ - Deduct amount                         │
└────────┬────────────────────────────────┘
         │
         ├─ Requests
         │
         ▼
┌─────────────────────────────────────────┐
│  INotificationService                   │
│  .SendAsync(notification)               │
│ (Email/SMS/Push or Composite)          │
└──────────┬────────────────────────────────┘
           │ sends notification
           ▼
        ┌────────────────┐
        │ Notification   │
        │ Sent to User   │
        └────────────────┘
         │
         ├─ Requests
         │
         ▼
┌─────────────────────────────────────────┐
│  ITransactionRepository                 │
│  .SaveTransactionAsync()                │
│ (InMemory/Database)                    │
└──────────┬────────────────────────────────┘
           │ persists transaction
           ▼
        ┌────────────────┐
        │ Transaction    │
        │ Saved          │
        └────────────────┘
```

## File Organization

```
CSharp_Wallet/
│
├── Models/
│   └── Transaction.cs
│       ├── Transaction class
│       ├── TransactionType enum
│       ├── TransactionStatus enum
│       └── PaymentMethod enum
│
├── Interfaces/  (Abstraction Layer)
│   ├── IPaymentProcessor.cs
│   │   ├── PaymentResult class
│   │   └── PaymentRequest class
│   ├── INotificationService.cs
│   │   ├── Notification class
│   │   └── NotificationType enum
│   └── ITransactionRepository.cs
│
├── Services/  (Implementation Layer)
│   ├── PaymentProcessors/
│   │   └── PaymentProcessors.cs
│   │       ├── UPIPaymentProcessor
│   │       ├── CardPaymentProcessor
│   │       └── NetBankingPaymentProcessor
│   ├── Notifications/
│   │   └── NotificationServices.cs
│   │       ├── EmailNotificationService
│   │       ├── SMSNotificationService
│   │       ├── PushNotificationService
│   │       └── CompositeNotificationService
│   └── Repository/
│       └── InMemoryTransactionRepository.cs
│
├── Core/  (Business Logic)
│   └── WalletService.cs
│       └── Main wallet orchestration
│
├── DependencyInjection/  (Configuration)
│   └── WalletServiceProvider.cs
│       └── Service builder & registration
│
├── Tests/  (Test Suite)
│   └── WalletServiceTests.cs
│       ├── Mock implementations
│       └── Unit tests with xUnit
│
└── Program.cs  (Application Entry Point)
    └── Demo application
```

## Testing Architecture

```
┌─────────────────────────────────────────┐
│      Unit Test Execution                │
│  WalletServiceTests                     │
└────────┬────────────────────────────────┘
         │
         ├─ Arrange: Create mocks
         │   ├─ MockPaymentProcessor
         │   ├─ MockNotificationService
         │   └─ MockRepository
         │
         ├─ Act: Call WalletService method
         │
         └─ Assert: Verify behavior
             (No real database/API/email)

Mock Objects Implement Real Interfaces:
┌─────────────────────────────────────┐
│  MockPaymentProcessor               │
│  implements IPaymentProcessor        │
├─────────────────────────────────────┤
│ ProcessPaymentAsync(): returns mock  │
│ ValidatePaymentDetails(): returns    │
│ configurable value                   │
└─────────────────────────────────────┘

Benefits of This Architecture:
✅ Unit tests run fast (no real API calls)
✅ Tests don't require database
✅ Tests don't send real emails
✅ Tests are isolated & deterministic
✅ Easy to test success & failure cases
```

## SOLID Principles Visualization

```
┌─────────────────────────────────────────────────┐
│  Single Responsibility Principle (SRP)          │
│  Each class has ONE reason to change            │
├─────────────────────────────────────────────────┤
│  WalletService:Only wallet operations          │
│  UPIPaymentProcessor: Only UPI logic            │
│  EmailService: Only email sending               │
│  Repository: Only data persistence              │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│  Open/Closed Principle (OCP)                    │
│  Open for extension, closed for modification   │
├─────────────────────────────────────────────────┤
│  Add new payment type: Create new processor     │
│  NO MODIFICATION to WalletService               │
│  Add new notification: Create new service       │
│  NO MODIFICATION to WalletService               │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│  Liskov Substitution Principle (LSP)            │
│  All implementations are substitutable         │
├─────────────────────────────────────────────────┤
│  UPIProcessor, CardProcessor, NetBankingProc    │
│  All work identically from caller perspective   │
│  Can swap at runtime                            │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│  Interface Segregation Principle (ISP)          │
│  Clients depend only on needed interfaces       │
├─────────────────────────────────────────────────┤
│  IPaymentProcessor - only payment methods       │
│  INotificationService - only notification      │
│  ITransactionRepository - only persistence     │
│  No fat interfaces!                             │
└─────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────┐
│  Dependency Inversion Principle (DIP)           │
│  Depend on abstractions, not concretions       │
├─────────────────────────────────────────────────┤
│  WalletService depends on:                      │
│  - IPaymentProcessor (abstraction)              │
│  - INotificationService (abstraction)           │
│  - ITransactionRepository (abstraction)         │
│  NOT on concrete implementations                │
└─────────────────────────────────────────────────┘
```

## Extension Points (Adding New Features)

```
Current Implementation    → Easy to Extend To:
────────────────────────    ──────────────────
UPI Payment              → Google Pay
Card Payment             → PayPal
Net Banking              → Bitcoin Payment

Email Notification       → Telegram
SMS Notification         → WhatsApp
Push Notification        → Slack

InMemory Repository      → SQL Server
                         → PostgreSQL
                         → MongoDB

All extensions achieved WITHOUT modifying WalletService!
```
