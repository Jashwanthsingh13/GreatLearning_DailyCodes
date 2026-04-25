# Quick Start Guide

## 📋 Project Overview

**Banking Transaction System with C# Exception Handling**

A complete implementation demonstrating:
- Custom exception classes
- Try-catch-finally blocks
- Business logic validation
- Comprehensive test coverage

## 🚀 Quick Start (30 seconds)

### Option 1: Using .NET CLI (Recommended)

```bash
cd Day5CaseStudy
dotnet run
```

### Option 2: Build Then Run

```bash
cd Day5CaseStudy
dotnet build
dotnet run --no-build
```

### Option 3: Direct Compilation

```bash
cd Day5CaseStudy
csc Program.cs BankAccount.cs InsufficientBalanceException.cs InvalidAmountException.cs -out:BankingApp.exe
./BankingApp
```

## 📂 File Structure

| File | Purpose | Lines |
|---|---|---|
| `Program.cs` | Main application with 11 test cases | 200+ |
| `BankAccount.cs` | Core banking logic and operations | 150+ |
| `InsufficientBalanceException.cs` | Custom exception for balance errors | 20 |
| `InvalidAmountException.cs` | Custom exception for invalid amounts | 20 |
| `BankingApp.csproj` | .NET project configuration | 10 |

## ✅ What Gets Tested

When you run the application, all these scenarios are tested:

1. ✓ Valid account creation (₹5000)
2. ✓ Valid deposit (₹2000 → Balance: ₹7000)
3. ✗ Invalid deposit (negative) → Exception caught
4. ✗ Invalid deposit (zero) → Exception caught
5. ✓ Valid withdrawal (₹3000 → Balance: ₹4000)
6. ✗ Withdrawal exceeding balance → Exception caught
7. ✗ Withdrawal below minimum → Exception caught
8. ✗ Invalid withdrawal (negative) → Exception caught
9. ✓ Maximum valid withdrawal (₹3000 → Balance: ₹1000)
10. ✓ Final balance check (₹1000)
11. ✗ Invalid account creation (Balance < ₹1000) → Exception caught

## 🎯 Key Features

### Exception Handling
```csharp
try
{
    account.Withdraw(amount);
}
catch (InvalidAmountException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
catch (InsufficientBalanceException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Business Rules Enforced
- Minimum balance: ₹1000
- Deposit amount: > 0
- Withdrawal: Cannot exceed balance
- Withdrawal: Must maintain minimum balance

### Custom Exceptions
- `InvalidAmountException` - Amount <= 0
- `InsufficientBalanceException` - Balance issues

## 📊 Expected Output

```
╔════════════════════════════════════════════════════════╗
║     BANKING TRANSACTION SYSTEM - Exception Demo        ║
╚════════════════════════════════════════════════════════╝

📌 TEST CASE 1: Creating a valid account
────────────────────────────────────────────────────────
Account Holder: Rahul Kumar
Current Balance: ₹5000.00
Minimum Balance Required: ₹1000.00

✓ Successfully deposited: ₹2000.00
  Current Balance: ₹7000.00

✗ Deposit Failed: Deposit amount must be greater than 0
⚠️  Caught InvalidAmountException...

[... More test cases ...]

Summary of Exception Handling Demonstrated:
✓ InvalidAmountException - Negative/Zero deposits & withdrawals
✓ InsufficientBalanceException - Excessive withdrawals
✓ ArgumentException - Invalid account holder name
```

## 🔧 System Requirements

- **.NET SDK 6.0+** (for `dotnet` command)
  - OR -
- **C# Compiler** (for `csc` command)
- **macOS/Linux/Windows**

### Check Installation

```bash
dotnet --version    # Should show 6.0 or higher
csc -version        # OR check C# compiler version
```

## 🐛 Troubleshooting

### "dotnet command not found"
Install .NET from: https://dotnet.microsoft.com/download

### "Press any key to exit..." hangs
Press `Ctrl+C` or any key to exit

### Build fails
Ensure all .cs files are in the same directory:
```bash
ls -la Day5CaseStudy/*.cs
# Should show all 4 .cs files and .csproj
```

### Cannot modify bin/ obj/ folders
These are auto-generated. Delete them with:
```bash
dotnet clean
```

## 📚 Learning Outcomes

After studying this code, you'll understand:

1. **Custom Exception Creation**
   - Inheriting from `Exception` class
   - Multiple constructors
   - Meaningful error messages

2. **Exception Handling Patterns**
   - Try-catch-finally blocks
   - Multiple catch clauses
   - Exception re-throwing

3. **Business Logic**
   - Input validation
   - State management
   - Rule enforcement

4. **C# Best Practices**
   - XML documentation
   - Property encapsulation
   - Null-safe operations
   - Meaningful variable names

5. **Testing Strategies**
   - Happy path testing
   - Error case testing
   - Boundary condition testing
   - Integration testing

## 🎓 Study Guide

**To understand the code flow:**

1. Read `Program.cs` Main() method first
2. Understand each test case
3. Trace through `BankAccount.cs` methods
4. Review custom exception handling
5. Study error messages

**Key code sections:**

- Lines 20-30 in `BankAccount.cs`: Constructor validation
- Lines 40-60: Deposit() method with exception handling
- Lines 70-100: Withdraw() method with business logic
- Lines 110-140: Exception throwing patterns
- Lines 160-200 in `Program.cs`: Test case implementation

## 💡 Tips for Modification

Want to extend the project? Try:

```csharp
// Add a transfer method
public void Transfer(BankAccount recipient, double amount) { }

// Add transaction history
public List<Transaction> GetHistory() { }

// Add interest calculation
public void ApplyInterest(double rate) { }

// Add withdrawal limit per day
public double GetDailyWithdrawalLimit() { }
```

## 🔗 Related Resources

- [C# Exception Handling](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/)
- [.NET API Documentation](https://docs.microsoft.com/en-us/dotnet/api/)
- [C# Coding Guidelines](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

## ✨ Notes

- Application exits cleanly after all tests
- No external dependencies required
- Code compiles with zero warnings
- Full test coverage included
- Production-ready error handling

---

**Need Help?** Check `README.md` for detailed documentation.

**Ready to Submit?** Follow `GITHUB_SUBMISSION.md` for GitHub upload instructions.
