using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using AdaCredit.Domain;

namespace AdaCredit.Data
{
    public static class FileManipulation
    {
        public static string ConfigFile(string fileName)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, fileName);
            return filePath;
        }

        public static void LoadClients()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                NewLine = Environment.NewLine
            };

            string fileName = "Clients.csv";
            var filePath = ConfigFile(fileName);

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvParser(reader, config);

                var items = new List<string>();

                csv.Read();

                while (csv.Read())
                {
                    string[] record = csv.Record;

                    var client = new Client(record[3])
                    {
                        Name = record[0],
                        PhoneNumber = record[1],
                        EmailAddress = record[2],
                        Active = record[4] == "True" ? true : false,
                        Account = new Account(record[6])
                        {
                            Balance = decimal.Parse(record[5], CultureInfo.InvariantCulture),
                            Agency = record[7],
                            CurrencySymbol = record[8]
                        }
                    };
                }

            } catch(Exception ex)
            {
                return;
            }
        }

        public static void SaveClients()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                NewLine = Environment.NewLine
            };

            string fileName = "Clients.csv";
            var filePath = ConfigFile(fileName);
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            csv.WriteHeader<Client>();
            csv.NextRecord();

            foreach (var client in ClientRepository.Clients)
            {
                csv.WriteRecord(client);
                csv.NextRecord();
            }
        }

        public static void LoadTransactions() 
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                NewLine = Environment.NewLine
            };

            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var transactionsFile = Path.Combine(desktopFolder, "Transactions/pending.csv");

            string dir = Path.Combine(desktopFolder, "Transactions");
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                using var reader = new StreamReader(transactionsFile);
                using var csv = new CsvParser(reader, config);

                var items = new List<string>();

                while (csv.Read())
                {
                    string[] record = csv.Record;

                    TransactionType transactionType = record[6] switch
                    {
                        string i when i == "DOC" => (TransactionType)0,
                        string i when i == "TED" => (TransactionType)1,
                        string i when i == "TEF" => (TransactionType)2
                        
                    };

                    var transaction = new Transaction
                    {
                        OriginBankCode = record[0],
                        OriginAgencyCode = record[1],
                        OriginBankAccount = record[2],
                        DestinationBankCode = record[3],
                        DestinationAgencyCode = record[4],
                        DestinationBankAccount = record[5],
                        TypeTransaction = transactionType,
                        Value = decimal.Parse(record[7], CultureInfo.InvariantCulture)
                    };

                    TransactionRepository.LoadTransaction(transaction);
                }

            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void SaveTransactions()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                NewLine = Environment.NewLine
            };

            string fileNameCompleted = "Transactions/completed.csv";
            var filePathCompleted = ConfigFile(fileNameCompleted);
            using var writerCompleted = new StreamWriter(filePathCompleted);
            using var csvCompleted = new CsvWriter(writerCompleted, config);

            csvCompleted.NextRecord();

            foreach (var sucessTransaction in TransactionRepository.CompletedTransactions)
            {
                csvCompleted.WriteRecord(sucessTransaction);
                csvCompleted.NextRecord();
            }

            string fileNameFailed = "Transactions/failed.csv";
            var filePathFailed = ConfigFile(fileNameFailed);
            using var writerFailed = new StreamWriter(filePathFailed);
            using var csvFailed = new CsvWriter(writerFailed, config);

            csvFailed.NextRecord();

            foreach (var failedTransaction in TransactionRepository.FailedTransactions)
            {
                csvFailed.WriteRecord(failedTransaction);
                csvFailed.NextRecord();
            }

            string fileName = "Transactions/pending.csv";
            var filePath = ConfigFile(fileName);
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            csv.NextRecord();

            foreach (var transaction in TransactionRepository.PendingTransactions)
            {
                csv.WriteRecord(transaction);
                csv.NextRecord();
            }
        }

        public static void LoadEmployees()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                NewLine = Environment.NewLine
            };

            string fileName = "Employees.csv";
            var filePath = ConfigFile(fileName);

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvParser(reader, config);

                var items = new List<string>();

                csv.Read();

                while (csv.Read())
                {
                    string[] record = csv.Record;

                    var employee = new Employee()
                    {
                        Name = record[0],
                        PhoneNumber = record[1],
                        EmailAddress = record[2],
                        Active = record[3] == "True" ? true : false,
                        Username= record[4],
                        HashPassword= record[5],
                        SaltPassword= record[6],
                        LastLogin = DateTime.Parse(record[7]),
                    };

                    EmployeeRepository.LoadEmployee(employee);
                }

            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void SaveEmployees()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                NewLine = Environment.NewLine
            };

            string fileName = "Employees.csv";
            var filePath = ConfigFile(fileName);
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);

            csv.WriteHeader<Employee>();
            csv.NextRecord();

            foreach (var employee in EmployeeRepository.Employees)
            {
                csv.WriteRecord(employee);
                csv.NextRecord();
            }
        }
    }
}
