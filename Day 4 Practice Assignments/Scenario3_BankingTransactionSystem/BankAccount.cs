using System;

namespace Scenario3_BankingTransactionSystem
{
    /// <summary>
    /// BankAccount class represents a customer's bank account
    /// </summary>
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public double Balance { get; set; }
        public string AccountType { get; set; }
        public DateTime OpenDate { get; set; }

        public BankAccount(string accountNumber, string accountHolder, double initialBalance, string accountType)
        {
            AccountNumber = accountNumber;
            AccountHolder = accountHolder;
            Balance = initialBalance;
            AccountType = accountType;
            OpenDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Account: {AccountNumber}, Holder: {AccountHolder}, Balance: ${Balance}, Type: {AccountType}";
        }
    }
}
