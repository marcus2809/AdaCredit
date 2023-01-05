using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Data;
using BCrypt.Net;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain
{
    public static class Login
    {
        public static bool Show() 
        {
            bool logged = false;
            bool usernameDontExists = true;
            string username;
            string cleanPassword;

            Console.Clear();
            Console.WriteLine("Insira seu username:");
            username = Console.ReadLine();

            var employee = EmployeeRepository.GetEmployeeByUsername(username);
            if (employee == null && username != "user")
            {
                Console.WriteLine("Usuário não encontrado");
                Console.WriteLine("Pressione enter para sair...");
                Console.ReadLine();
                return false;
            }

            do
            {
                Console.Clear();
                Console.WriteLine("Insira sua senha:");
                cleanPassword = Console.ReadLine();

                if (username == "user" && cleanPassword == "pass")
                    logged = true;
                else if (username != "user")
                {
                    var concatPassword = string.Concat(cleanPassword, employee.SaltPassword);

                    logged = Verify(concatPassword, employee.HashPassword);
                }

            } while (!logged);

            if (employee != null) employee.LoginUser();

            Console.Clear();
            Console.WriteLine("Usuário logado");
            Console.WriteLine("Pressione enter para ir para o menu...");
            Console.ReadLine();

            return true;
        }

        public static string GenerateHashPassword(string saltPassword)
        {
            Console.WriteLine("Digite sua senha: ");
            var cleanPassword = Console.ReadLine();
            var concatPassword = string.Concat(cleanPassword, saltPassword);
            var hashedPassword = HashPassword(concatPassword);
            //Console.WriteLine($"O hash para sua senha é: {hashedPassword}");

            var passwordsMatch = false;

            do
            {
                Console.WriteLine("Confirme sua senha: ");
                passwordsMatch = Verify(string.Concat(Console.ReadLine(), saltPassword), hashedPassword);
                Console.WriteLine($"A verificação foi {(passwordsMatch ? "positiva" : "negativa")}");

            } while (!passwordsMatch);

            //Console.Clear();
            Console.WriteLine("Senha cadastrada");

            Console.WriteLine("Pressione enter para sair...");
            Console.ReadLine();

            return hashedPassword;
        }
    }
}
