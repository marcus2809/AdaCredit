using AdaCredit.Data;
using AdaCredit.Domain;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.View
{
    public class ClientView
    {
        public void RegisterClient()
        {
            Console.Clear();
            Console.WriteLine("Insira o nome completo do cliente:");
            var name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o CPF:");
            var document = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o telefone:");
            var phoneNumber = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o Email:");
            var emailAddress = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Deseja registrar o cliente com os seguintes dados ?\n");
            Console.WriteLine($"Nome: {name}\nCPF: {document}\nTelefone: {phoneNumber}\nEmail: {emailAddress}\n");
            Console.WriteLine("Pressione enter para cadastrar o cliente...");
            Console.ReadLine();

            var newClient = new Client()
            {
                Name = name,
                Document = document,
                PhoneNumber = phoneNumber,
                EmailAddress = emailAddress
            };

            Console.Clear();
            Console.WriteLine("Cliente cadastrado\n"); // checar se cliente ja existe
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }

        public void ViewAllActiveClients()
        {
            Console.Clear();
            var activeClients = ClientRepository.GetAllActiveClients();

            foreach (var client in activeClients) 
                Console.WriteLine($"{client.Name}, {client.Account.CurrencySymbol}{client.Account.Balance}");
            
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }

        public void ViewAllInactiveClients()
        { 
            Console.Clear();
            var inactiveClients = ClientRepository.GetAllInactiveClients();

            foreach (var clients in inactiveClients)
                Console.WriteLine(clients);

            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }

        public void ViewClientByDocument() 
        {
            Console.Clear();
            Console.WriteLine("Insira o CPF do cliente:");
            var document = Console.ReadLine();
            Console.Clear();

            var client = ClientRepository.GetClientByDocument(document);

            if (client == null)
            {
                Console.WriteLine("Cliente com o dado CPF não existe\n");
                Console.WriteLine("Pressione enter para sair...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"Nome: {client.Name}\nCPF: {document}\nTelefone: {client.PhoneNumber}\nEmail: {client.EmailAddress}\n" +
                $"{(client.Active ? "Ativo" : "Inativo")}\n");

            Console.WriteLine("Precione enter para sair...");
            Console.ReadLine();
        }

        public void ViewDeactivateClient()
        {
            Console.Clear();
            Console.WriteLine("Insira o CPF do cliente que deseja desativar:");
            var document = Console.ReadLine();
            Console.Clear();

            var client = ClientRepository.GetClientByDocument(document);

            if (client == null)
            {
                Console.WriteLine("Cliente com o dado CPF não existe\n");
                Console.WriteLine("Pressione enter para sair...");
                Console.ReadLine();
                return;
            }

            client.Deactivate();

            Console.WriteLine("Cadastro do cliente desativado\n");
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();

        }

        public void ViewUpdateClient()
        {
            Console.Clear();
            Console.WriteLine("Insira o CPF do cliente:");
            var document = Console.ReadLine();
            Console.Clear();

            var client = ClientRepository.GetClientByDocument(document);

            if (client == null)
            {
                Console.WriteLine("Cliente com o dado CPF não existe\n");
                Console.WriteLine("Pressione enter para sair...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Nome: {client.Name}\nCPF: {document}\nTelefone: {client.PhoneNumber}\nEmail: {client.EmailAddress}\n" +
                $"{(client.Active ? "Ativo" : "Inativo")}\n");

            Console.WriteLine("Pressione enter para alterar o cadastro do cliente...");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Insira o nome completo do cliente: (Deixe em branco caso não queira alterar)");
            var name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o telefone: (Deixe em branco caso não queira alterar)");
            var phoneNumber = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o Email: (Deixe em branco caso não queira alterar)");
            var emailAddress = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Deseja registrar o cliente com os seguintes dados ?\n");
            Console.WriteLine
            (
                $"Nome: {(name != "" ? name : client.Name + " (Inalterado)")}\n" +
                $"CPF: {document} (Inalterado)\n" +
                $"Telefone: {(phoneNumber != "" ? phoneNumber : client.PhoneNumber + " (Inalterado)")}\n" +
                $"Email: {(emailAddress != "" ? emailAddress : client.EmailAddress + " (Inalterado)")}\n"
            );
            Console.WriteLine("Pressione enter para confirmar a alteraração o cadastro do cliente...");
            Console.ReadLine();

            client.UpdateName(name);
            client.UpdatePhoneNumber(phoneNumber);
            client.UpdateEmailAddress(emailAddress);

            Console.Clear();
            Console.WriteLine("Cadastro do cliente alterado\n");
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }
    }
}
