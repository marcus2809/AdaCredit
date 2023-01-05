using AdaCredit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain
{
    public sealed class Client
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Document { get; set; }
        public bool Active { get; set; } = true;
        public Account Account { get; set; }
        public Client() 
        {
            this.Account = new Account();
            ClientRepository.CreateClient(this);
        }
        public Client(string Document)
        {
            this.Document = Document;
            ClientRepository.LoadClient(this);
        }
        public override string ToString()
            => Name;

        public void Deactivate()
            => this.Active = false;

        public void UpdateName(string name)
            => this.Name = name != "" ? name : this.Name;

        public void UpdatePhoneNumber(string phoneNumber)
            => this.PhoneNumber = phoneNumber != "" ? phoneNumber : this.PhoneNumber;

        public void UpdateEmailAddress(string emailAddress)
            => this.EmailAddress = emailAddress != "" ? emailAddress : this.EmailAddress;
    }
}
