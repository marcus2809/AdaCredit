using AdaCredit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Data
{
    public static class EmployeeRepository
    {
        public static List<Employee> Employees { get; set; } = new List<Employee>();

        public static void LoadEmployee(Employee employee)
            => Employees.Add(employee);

        public static bool CreateEmployee(Employee employee)
        {
            Employees.Add(employee);
            return true;
        }

        public static Employee? GetEmployeeByUsername(string username)
            => Employees.FirstOrDefault(e => e.Username == username);
        

        public static bool ExistsEmployeeByUsername(string userName)
            => Employees.Any(e => e.Username == userName);   
        
        public static List<Employee> GetAllActiveEmployees()
        {
            return Employees.Where(e => e.Active).ToList();
        }

    }
}
