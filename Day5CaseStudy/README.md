# C# Banking Transaction System with Exception Handling

A comprehensive C# case study demonstrating exception handling, custom exceptions, and proper error management in a banking application.

## Problem Statement

Develop a Banking Transaction System where users can perform banking operations (deposit, withdraw, check balance) while maintaining business rules and handling invalid scenarios using exception handling.

### Business Rules
- Minimum account balance: ₹1000
- Withdrawal amount cannot exceed available balance
- Deposit amount must be greater than 0
- Invalid inputs must throw appropriate exceptions

## Features

### 1. Custom Exceptions
- **`InsufficientBalanceException`**: Raised when withdrawal would result in balance below minimum or exceeds available balance
- **`InvalidAmountException`**: Raised when deposit/withdrawal amount is invalid (≤ 0)

### 2. Built-in Exceptions Handled
- `ArgumentException`: Invalid account holder name
- `InvalidOperationException`: Generic operation failures
- `Exception`: General error handling

### 3. BankAccount Class

#### Properties
- `AccountHolderName` (string): Name of the account holder
- `Balance` (double): Current account balance

#### Methods
- `Deposit(double amount)`: Add money to account
- `Withdraw(double amount)`: Remove money from account
- `CheckBalance()`: Display current balance information

## Exception Handling Strategy

### Try-Catch-Finally Structure
```csharp
try
{
    // Perform operation
    account.Withdraw(amount);
}
catch (InvalidAmountException ex)
{
    // Handle invalid amount
}
catch (InsufficientBalanceException ex)
{
    // Handle insufficient balance
}
catch (Exception ex)
{
    // Handle unexpected errors
}
finally
{
    // Cleanup (if needed)
}
```

## User Stories Implementation

| ID | User Story | Status |
|---|---|---|
| US1 | Deposit money with validation | ✓ Implemented |
| US2 | Withdraw money with balance update | ✓ Implemented |
| US3 | Prevent withdrawal beyond limit | ✓ Implemented |
| US4 | Handle invalid deposit amounts | ✓ Implemented |
| US5 | Enforce business rules with custom exceptions | ✓ Implemented |

## Test Cases

### Test Case 1: Valid Account Creation
```
Create account "Rahul Kumar" with initial balance ₹5000
Expected: Account created successfully
```

### Test Case 2: Valid Deposit
```
Deposit: ₹2000
Expected: Balance updated to ₹7000
```

### Test Case 3: Invalid Deposit (Negative)
```
Deposit: ₹-500
Expected: InvalidAmountException thrown
```

### Test Case 4: Invalid Deposit (Zero)
```
Deposit: ₹0
Expected: InvalidAmountException thrown
```

### Test Case 5: Valid Withdrawal
```
Withdraw: ₹3000
Expected: Balance updated, remains above minimum
```

### Test Case 6: Withdrawal Exceeding Balance
```
Current Balance: ₹4000
Withdraw: ₹10000
Expected: InsufficientBalanceException thrown
```

### Test Case 7: Withdrawal Below Minimum Balance
```
Current Balance: ₹4000
Withdraw: ₹3500 (would result in ₹500)
Expected: InsufficientBalanceException thrown (below ₹1000 minimum)
```

### Test Case 8: Invalid Withdrawal (Negative)
```
Withdraw: ₹-1000
Expected: InvalidAmountException thrown
```

## Sample Output

```
╔════════════════════════════════════════════════════════╗
║     BANKING TRANSACTION SYSTEM - Exception Demo        ║
╚════════════════════════════════════════════════════════╝

📌 TEST CASE 1: Creating a valid account
────────────────────────────────────────────────────────
Account Holder: Rahul Kumar
Current Balance: ₹5000.00
Minimum Balance Required: ₹1000.00

📌 TEST CASE 2: Valid deposit of ₹2000
────────────────────────────────────────────────────────
✓ Successfully deposited: ₹2000.00
  Current Balance: ₹7000.00

📌 TEST CASE 3: Invalid deposit of ₹-500 (negative amount)
────────────────────────────────────────────────────────
✗ Deposit Failed: Deposit amount must be greater than 0. Provided: ₹-500
⚠️  Caught InvalidAmountException: Deposit amount must be greater than 0. Provided: ₹-500

📌 TEST CASE 5: Valid withdrawal of ₹3000
────────────────────────────────────────────────────────
✓ Successfully withdrawn: ₹3000.00
  Current Balance: ₹4000.00

📌 TEST CASE 6: Withdrawal of ₹10000 (exceeds balance)
────────────────────────────────────────────────────────
✗ Withdrawal Failed: Withdrawal amount (₹10000.00) exceeds available balance (₹4000.00).
⚠️  Caught InsufficientBalanceException: Withdrawal amount (₹10000.00) exceeds available balance (₹4000.00).

📌 TEST CASE 7: Withdrawal of ₹3500 (would go below minimum balance)
────────────────────────────────────────────────────────
Current balance: ₹4000.00
Attempting to withdraw ₹3500 (would result in ₹500, below ₹1000 minimum)

✗ Withdrawal Failed: Withdrawal would result in balance below minimum (₹1000). Current Balance: ₹4000.00, Minimum Required: ₹1000, Maximum Withdrawal: ₹3000.00
⚠️  Caught InsufficientBalanceException: Withdrawal would result in balance below minimum (₹1000). Current Balance: ₹4000.00, Minimum Required: ₹1000, Maximum Withdrawal: ₹3000.00
```

## How to Run

### Prerequisites
- .NET SDK installed (version 6.0 or higher)
- C# compiler available

### Compilation Steps

1. **Navigate to project directory:**
   ```bash
   cd Day5CaseStudy
   ```

2. **Compile the project:**
   ```bash
   csc Program.cs BankAccount.cs InsufficientBalanceException.cs InvalidAmountException.cs
   ```
   
   Or using .NET CLI:
   ```bash
   dotnet new console
   dotnet build
   ```

3. **Run the application:**
   ```bash
   ./Program
   ```
   
   Or on Windows:
   ```bash
   Program.exe
   ```

### Or Run Directly
```bash
csc Program.cs BankAccount.cs InsufficientBalanceException.cs InvalidAmountException.cs -out:BankingApp.exe && ./BankingApp
```

## Project Structure

```
Day5CaseStudy/
├── Program.cs                              # Main application with test cases
├── BankAccount.cs                          # Core banking logic and operations
├── InsufficientBalanceException.cs          # Custom exception for balance errors
├── InvalidAmountException.cs                # Custom exception for amount validation
└── README.md                               # Documentation
```

## Exception Types Used

| Exception Type | Scenario | Handler |
|---|---|---|
| `InvalidAmountException` | Deposit/Withdraw <= 0 | Catch InvalidAmountException |
| `InsufficientBalanceException` | Withdraw > balance | Catch InsufficientBalanceException |
| `InsufficientBalanceException` | Withdraw causes balance < ₹1000 | Catch InsufficientBalanceException |
| `ArgumentException` | Invalid account holder name | Catch Exception (ArgumentException) |

## Code Quality Features

✓ **Proper Exception Hierarchy**: Custom exceptions inherit from Exception  
✓ **Descriptive Error Messages**: Clear, actionable error information  
✓ **Input Validation**: All inputs validated before processing  
✓ **Try-Catch-Finally**: Comprehensive error handling throughout  
✓ **XML Documentation**: Class and method documentation included  
✓ **User-Friendly Output**: Formatted console output with visual indicators  
✓ **Business Logic Enforcement**: All rules correctly implemented  
✓ **Edge Case Handling**: Minimum balance and zero amount checks  


## Key Learning Outcomes

1. **Custom Exception Creation**: Implemented domain-specific exceptions
2. **Exception Handling**: Proper try-catch-finally usage patterns
3. **Business Logic**: Validation rules enforcement
4. **Error Messages**: Clear, helpful error communication
5. **Code Organization**: Clean class structure and separation of concerns
6. **Testing**: Comprehensive test scenarios covering happy path and error cases

## Author
Great Learning Case Study Implementation

## License
Proprietary content - Great Learning. All Rights Reserved.
Unauthorized use or distribution prohibited.
