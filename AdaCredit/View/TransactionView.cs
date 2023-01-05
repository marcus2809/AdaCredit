using AdaCredit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.View
{
    public class TransactionView
    {
        public void ViewProcessTransactions()
        {
            Console.Clear();
            Console.WriteLine("Processando Transações...");

            TransactionRepository.ProcessTransactions();

            Console.WriteLine("\nTransações processadas");
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }

        public void ViewFailedTransactions()
        {
            Console.Clear();
            Console.WriteLine("Origem|Destino|Tipo|Valor|Detalhes");
            foreach (var transaction in TransactionRepository.FailedTransactions)
            {
                Console.WriteLine(transaction);
            }

            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }
    }
}
