using AdaCredit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Data
{
    public static class TransactionRepository
    {
        public static List<Transaction> PendingTransactions { get; set; } = new List<Transaction>();
        public static List<Transaction> CompletedTransactions { get; set; } = new List<Transaction>();
        public static List<Transaction> FailedTransactions { get; set; } = new List<Transaction>();

        public static void LoadTransaction(Transaction transaction)
            => PendingTransactions.Add(transaction);  
        

        public static void ProcessTransactions() 
        {
            foreach (var transaction in PendingTransactions)
            {
                var completed = transaction.ProcessTransaction();
                if (completed) CompletedTransactions.Add(transaction);
                else FailedTransactions.Add(transaction);
            }
            PendingTransactions.Clear();
        }

        public static void AddCompletedTransaction(Transaction transaction)
            => CompletedTransactions.Add(transaction);

        public static void AddFailedTransaction(Transaction transaction)
            => FailedTransactions.Add(transaction);
    }
}
