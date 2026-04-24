# Day 4 Practice Assignments - C# Collections & OOP

This directory contains 5 comprehensive C# projects demonstrating the use of various collection types and Object-Oriented Programming (OOP) principles.

## Scenarios Overview

### 📦 Scenario 1: E-Commerce Order Management System
**Location:** `Scenario1_ECommerceOrderManagement/`

**Collections Used:**
- `List<Order>` - Store all orders placed
- `Dictionary<int, Customer>` - Map customer ID to customer details
- `HashSet<string>` - Store unique product categories
- `Queue<Order>` - Order processing (FIFO)
- `Stack<string>` - Track order status history (LIFO)

**OOP Features:**
- Custom `Order` and `Customer` classes
- `IOrderManagement` interface
- Constructors for object initialization
- Static constants (`SYSTEM_NAME`, `MAX_ORDERS`)
- Static method `DisplaySystemInfo()`

**Key Methods:**
- `AddOrder()`, `RemoveOrder()`, `UpdateOrderStatus()`
- `ProcessOrders()` - FIFO processing
- `DisplayStatusHistory()` - LIFO history tracking

---

### 👥 Scenario 2: Social Media Platform (User Engagement System)
**Location:** `Scenario2_SocialMediaPlatform/`

**Collections Used:**
- `List<string>` - Store posts
- `Dictionary<string, int>` - Track likes per post
- `HashSet<int>` - Track unique user IDs
- `Stack<string>` - Track recent actions (undo functionality)
- `Queue<string>` - Process notifications (FIFO)

**OOP Features:**
- Custom `User` class
- `IEngagementSystem` interface
- Constructors with parameters
- Static constants for platform limits
- Enum-like tracking for user registration

**Key Methods:**
- `AddPost()`, `LikePost()`, `RemovePost()`
- `UndoLastAction()` - Stack-based undo
- `ProcessNotifications()` - Queue-based notification handling

---

### 💳 Scenario 3: Banking Transaction System
**Location:** `Scenario3_BankingTransactionSystem/`

**Collections Used:**
- `List<Transaction>` - Store transaction history
- `Dictionary<string, double>` - Store account balances
- `Queue<Transaction>` - Queue for pending transactions
- `Stack<Transaction>` - Stack for rollback operations
- `HashSet<string>` - Ensure unique transaction IDs

**OOP Features:**
- Custom `Transaction` and `BankAccount` classes
- `IBankingSystem` interface
- Constructors for initialization
- Static constants for security (`MIN_BALANCE`, `TRANSACTION_LIMIT`)
- Validation methods

**Key Methods:**
- `ProcessTransaction()` - Queue-based transaction handling
- `ExecutePendingTransactions()` - FIFO execution
- `RollbackLastTransaction()` - Stack-based rollback
- `UpdateBalance()` - Account management

---

### 🎵 Scenario 4: Music Playlist Manager (Advanced Collections)
**Location:** `Scenario4_MusicPlaylistManager/`

**Collections Used:**
- `LinkedList<string>` - Playlist management (easy insertion/removal)
- `SortedList<int, string>` - Songs sorted by rating
- `SortedDictionary<string, string>` - Artist → Song (sorted by artist)

**OOP Features:**
- Custom `Song` class with properties
- `IPlaylistManager` interface
- Constructors with multiple parameters
- Static constants for app configuration

**Key Methods:**
- `AddSong()`, `RemoveSong()` - LinkedList operations
- `DisplaySortedByRating()` - SortedList usage
- `DisplayByArtist()` - SortedDictionary usage
- `PlaySong()` - Play history tracking

---

### 📋 Scenario 5: Task Scheduler System
**Location:** `Scenario5_TaskSchedulerSystem/`

**Collections Used:**
- `Queue<string>` - Task execution order (FIFO)
- `Stack<string>` - Undo last executed task (LIFO)
- `List<string>` - All tasks
- `SortedDictionary<int, string>` - Priority-based tasks (sorted)
- `HashSet<string>` - Ensure no duplicate tasks

**OOP Features:**
- Custom `Task` class with enum `TaskStatus`
- `ITaskScheduler` interface
- Constructors with validation
- Static constants for system limits
- Enum for task states

**Key Methods:**
- `AddTask()`, `RemoveTask()` - Task management
- `ExecuteNextTask()`, `ExecuteAllTasks()` - FIFO execution
- `UndoLastTask()` - Stack-based undo
- `DisplayByPriority()` - SortedDictionary usage

---

## OOP Features Implemented Across All Scenarios

### 1. **Classes & Objects**
- Custom domain classes for each scenario (Order, Customer, User, Transaction, etc.)
- Proper encapsulation with properties and methods

### 2. **Constructors**
- Parameterized constructors for object initialization
- Default values where appropriate

### 3. **Interfaces**
- `IOrderManagement` in Scenario 1
- `IEngagementSystem` in Scenario 2
- `IBankingSystem` in Scenario 3
- `IPlaylistManager` in Scenario 4
- `ITaskScheduler` in Scenario 5

### 4. **Static Members**
- Static constants for system configuration (SYSTEM_NAME, MAX_ITEMS, etc.)
- Static methods for displaying system information

### 5. **Enums**
- `TaskStatus` enum in Scenario 5 for task states

### 6. **Collections**
All implementations use appropriate collection types:
- **List<T>** - For sequential storage
- **Dictionary<K,V>** - For key-value mappings
- **HashSet<T>** - For unique values
- **Queue<T>** - For FIFO operations
- **Stack<T>** - For LIFO operations
- **LinkedList<T>** - For efficient insertion/removal
- **SortedList<K,V>** - For sorted sequential storage
- **SortedDictionary<K,V>** - For sorted key-value pairs

---

## How to Run

### Prerequisites
- .NET Framework or .NET Core installed
- C# compiler (csc.exe) or Visual Studio

### Compilation
Navigate to each scenario folder and compile:

```bash
cd Scenario1_ECommerceOrderManagement
csc Program.cs Order.cs Customer.cs OrderManagementSystem.cs
./Program.exe
```

Or in Visual Studio:
1. Open each scenario folder
2. Compile all .cs files together
3. Run the executable

### Expected Output
Each program demonstrates:
- Object creation and management
- Collection manipulation (add, remove, update)
- FIFO and LIFO operations
- Display of statistics and results

---

## Key Learning Points

1. **List** - Dynamic arrays with index-based access
2. **Dictionary** - Fast lookups with key-value pairs
3. **HashSet** - Unique values, set operations
4. **Queue** - FIFO (First-In-First-Out) processing
5. **Stack** - LIFO (Last-In-First-Out) processing
6. **LinkedList** - Efficient insertion/removal
7. **SortedList/SortedDictionary** - Automatic sorting

---

## Notes

- All scenarios include proper input validation
- Error messages provide clear feedback
- Statistics display shows collection state
- Comments explain collection usage patterns
- Code follows C# naming conventions and best practices

Created for Great Learning Daily Code Challenge - Day 4
