using AdaCredit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Data
{
    public static class ClientRepository
    {
        public static List<Client> Clients { get; set; } = new List<Client>();

        public static void LoadClient(Client client)
            => Clients.Add(client);
        
        public static bool CreateClient(Client client)
        {
            if (ExistsByDocument(client.Document))
                return false;
            Clients.Add(client);
            return true;
        }

        public static List<Client> GetAllActiveClients() 
        { 
            List<Client> activeClients = new List<Client>();

            foreach (Client client in Clients)
                if (client.Active) activeClients.Add(client);

            return activeClients;
        }
        
        public static List<Client> GetAllInactiveClients() 
        {
            List<Client> inactiveClients = new List<Client>();

            foreach (Client client in Clients)
                if (!client.Active) inactiveClients.Add(client);
            
            return inactiveClients;
        }

        public static Client? GetClientByDocument(string document)
            => Clients.FirstOrDefault(c => c.Document == document);
        
        public static Client? GetClientByBankAccount(string bankAccount)
            => Clients.FirstOrDefault(c => c.Account.BankAccount == bankAccount);

        public static bool ExistsByDocument(string document)
            => Clients.Any(c => c.Document == document);
        
    }
}
