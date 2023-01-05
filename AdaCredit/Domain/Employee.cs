using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain
{
    public sealed class Employee
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool Active { get; set; } = true;
        public string Username { get; set; }
        public string HashPassword { get; set; }
        public string SaltPassword { get; set; }
        public DateTime LastLogin { get; set; }

        public Employee(string username) 
        {
            this.Username = username;
            this.SaltPassword = GenerateSalt();
            this.HashPassword = Login.GenerateHashPassword(SaltPassword);
        }

        public Employee() { }

        public void LoginUser()
            => LastLogin = DateTime.Now;

        public void UpdatePassword()
        {
            this.SaltPassword = GenerateSalt();
            this.HashPassword = Login.GenerateHashPassword(SaltPassword);
        }

        public void Deactivate()
            => Active = false;
    }
}
