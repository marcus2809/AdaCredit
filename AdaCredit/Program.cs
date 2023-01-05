using AdaCredit.Data;
using AdaCredit.Domain;
using AdaCredit.View;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using ConsoleTools;

namespace AdaCredit
{
    public class Program
    {
        static void Main(string[] args)
        {
            LoadAllData();
            GenerateClients();
            GenerateTransactions();

            var clientView = new ClientView();
            var transactionView = new TransactionView();
            var employeeView = new EmployeeView();

            var subMenuClient = new ConsoleMenu(args, level: 2)
                .Add("Cadastrar Novo Cliente", () => clientView.RegisterClient())
                .Add("Consultar os Dados de um Cliente existente", () => clientView.ViewClientByDocument())
                .Add("Alterar o Cadastro de um Cliente existente", () => clientView.ViewUpdateClient())
                .Add("Desativar Cadastro de um Cliente existente", () => clientView.ViewDeactivateClient())
                .Add("Voltar para o Menu", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableFilter = true;
                    config.Title = "Clientes";
                    config.EnableBreadcrumb = true;
                    config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                });

            var subMenuEmployee = new ConsoleMenu(args, level: 2)
                .Add("Cadastrar Novo Funcionário", () => employeeView.ViewRegisterEmployee())
                .Add("Alterar Senha de um Funcionário existente", () => employeeView.ViewUpdatePassword())
                .Add("Desativar Cadastro de um Funcionário existente", () => employeeView.ViewDeactivateEmployee())
                .Add("Voltar para o Menu", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableFilter = true;
                    config.Title = "Funcionários";
                    config.EnableBreadcrumb = true;
                    config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                });

            var subMenuTransactions = new ConsoleMenu(args, level: 2)
                .Add("Processar Transações (Reconciliação Bancária)", () => transactionView.ViewProcessTransactions())
                .Add("Voltar para o Menu", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableFilter = true;
                    config.Title = "Transações";
                    config.EnableBreadcrumb = true;
                    config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                });

            var subMenuReports = new ConsoleMenu(args, level: 2)
                .Add("Exibir Todos os Clientes Ativos com seus Respectivos Saldos", () => clientView.ViewAllActiveClients())
                .Add("Exibir Todos os Clientes Inativos", () => clientView.ViewAllInactiveClients())
                .Add("Exibir Todos os Funcionários Ativos e sua Última Data e Hora de Login", () => employeeView.ViewAllActiveEmployees())
                .Add("Exibir Transações com Erro (Detalhes da transação e do Erro)", () => transactionView.ViewFailedTransactions())
                .Add("Voltar para o Menu", ConsoleMenu.Close)
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableFilter = true;
                    config.Title = "Relatórios";
                    config.EnableBreadcrumb = true;
                    config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                });

            var menu = new ConsoleMenu(args, level: 1)
                .Add("Clientes", subMenuClient.Show)
                .Add("Funcionários", subMenuEmployee.Show)
                .Add("Transações", subMenuTransactions.Show)
                .Add("Relatórios", subMenuReports.Show)
                .Add("Sair e fazer login novamente", ConsoleMenu.Close)
                .Add("Fechar Programa", () => ExitProgram())
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableFilter = true;
                    config.Title = "Menu";
                    config.EnableBreadcrumb = true;
                    config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
                });

            var login = new ConsoleMenu(args, level: 0)
                .Add("Fazer login", (thisMenu) => 
                {
                    if (Login.Show())
                        menu.Show();
                })
                .Add("Fechar Programa", () => ExitProgram())
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableFilter = true;
                    config.Title = "Ada Credit";
                    config.EnableWriteTitle = true;
                    config.EnableBreadcrumb = true;
                });

            login.Show();
        }

        private static void LoadAllData()
        {
            FileManipulation.LoadClients();
            FileManipulation.LoadEmployees();
            FileManipulation.LoadTransactions();
        }

        private static void ExitProgram()
        {
            FileManipulation.SaveClients();
            FileManipulation.SaveEmployees();
            FileManipulation.SaveTransactions();
            Environment.Exit(0);
        }

        static void GenerateClients()
        {
            int numberOfClients = 50;
            bool active = false;

            var testClients = new Faker<Client>("pt_BR")
                .RuleFor(c => c.Name, faker => faker.Name.FullName(faker.PickRandom<Name.Gender>()))
                .RuleFor(c => c.Document, faker => faker.Person.Cpf())
                .RuleFor(c => c.EmailAddress, faker => faker.Person.Email)
                .RuleFor(c => c.PhoneNumber, faker => faker.Person.Phone)
                .Generate(numberOfClients);

            var testAccounts = new Faker<Account>()
                .RuleFor(a => a.BankAccount, faker => faker.Random.ReplaceNumbers("#####-#"))
                .Generate(numberOfClients);

            for (int i = 0; i < numberOfClients; i++)
            {
                testClients[i].Account = testAccounts[i];

                Console.WriteLine(testClients[i]);
                if (active)
                {
                    testClients[i].Active = false;
                    active = false;
                }
                else
                    active = true;
            }
        }

        static void GenerateTransactions()
        {
            var transactions = new Faker<Transaction>("pt_BR")
                .RuleFor(t => t.OriginBankCode, faker => faker.Random.ReplaceNumbers("###"))
                .RuleFor(t => t.OriginAgencyCode, faker => faker.Random.ReplaceNumbers("####"))
                .RuleFor(t => t.OriginBankAccount, faker => faker.PickRandom(ClientRepository.Clients).Account.BankAccount.Replace("-", ""))
                .RuleFor(t => t.DestinationBankCode, faker => faker.Random.ReplaceNumbers("###"))
                .RuleFor(t => t.DestinationAgencyCode, faker => faker.Random.ReplaceNumbers("####"))
                .RuleFor(t => t.DestinationBankAccount, faker => faker.Random.ReplaceNumbers("######"))
                .RuleFor(t => t.TypeTransaction, faker => faker.PickRandom<TransactionType>())
                .RuleFor(t => t.Value, faker => decimal.Round(faker.Random.Decimal(0.0M, 9999.99M), 2))
                .Generate(500);

            foreach (var transaction in transactions)
            {
                TransactionRepository.LoadTransaction(transaction);
            }
        }
    }
}