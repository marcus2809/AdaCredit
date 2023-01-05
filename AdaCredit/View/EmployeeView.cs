using AdaCredit.Data;
using AdaCredit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdaCredit.View
{
    public class EmployeeView
    {
        public void ViewRegisterEmployee()
        {
            Console.Clear();
            Console.WriteLine("Insira o nome completo do funcionário:");
            var name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o telefone do funcionário:");
            var phoneNumber = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Insira o endereço de email do funcionário:");
            var emailAddress = Console.ReadLine();

            Console.Clear();

            bool usernameExists;
            string username;

            do
            {
                Console.WriteLine("Insira o username do funcionário:");
                username = Console.ReadLine();
                usernameExists = EmployeeRepository.ExistsEmployeeByUsername(username);
                if (usernameExists) Console.WriteLine("username já está em uso");

            } while (usernameExists);

            Console.Clear();
            Console.WriteLine("Deseja registrar o funcionário com os seguintes dados ?\n");
            Console.WriteLine($"Nome: {name}\nUsername: {username}\nTelefone: {phoneNumber}\nEmail: {emailAddress}\n");
            Console.WriteLine("Pressione enter para cadastrar o funcionário...");
            Console.ReadLine();

            var newEmployee = new Employee(username)
            {
                Name = name,
                PhoneNumber = phoneNumber,
                EmailAddress = emailAddress
            };

            EmployeeRepository.CreateEmployee(newEmployee);

            Console.Clear();
            Console.WriteLine("Funcionario cadastrado\n");
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }

        public void ViewUpdatePassword()
        {
            Console.Clear();
            Console.WriteLine("Insira o username do funcionário:");
            var username = Console.ReadLine();

            var employee = EmployeeRepository.GetEmployeeByUsername(username);
            if(employee == null)
            {
                Console.WriteLine("Funcionário não existe");
                Console.WriteLine("Pressione enter para sair...");
                Console.ReadLine();
                return;
            }

            employee.UpdatePassword();
        }

        public void ViewDeactivateEmployee()
        {
            Console.Clear();
            Console.WriteLine("Insira o username do funcionário:");
            var username = Console.ReadLine();

            var employee = EmployeeRepository.GetEmployeeByUsername(username);
            if(employee == null)
            {
                Console.WriteLine("Funcionário não existe");
                Console.WriteLine("Pressione enter para sair...");
                Console.ReadLine();
                return;
            }
            employee.Deactivate();
            Console.WriteLine("Cadastro do funcionário desativado\n");
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }

        public void ViewAllActiveEmployees()
        {
            var activeEmplyees = EmployeeRepository.GetAllActiveEmployees();

            Console.Clear();

            foreach (var employee in activeEmplyees)
                Console.WriteLine($"{employee.Name}, {employee.Username}, {employee.LastLogin}");
            
            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();
        }
    }
}
