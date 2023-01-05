

using Bogus;

namespace AdaCredit.Domain
{
    public sealed class Account
    {
        public decimal Balance { get; set; }
        public string BankAccount { get; set; }
        public string Agency { get; set; } = "0001";
        public string CurrencySymbol { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Account() 
        {
            this.Balance = 0M;
            this.CurrencySymbol = "R$";
            this.Transactions = new List<Transaction>();
        }

        public Account(string bankAccount) 
        {
            this.BankAccount = bankAccount;
            this.CurrencySymbol = "R$";
            this.Transactions = new List<Transaction>();
        }
    }
}