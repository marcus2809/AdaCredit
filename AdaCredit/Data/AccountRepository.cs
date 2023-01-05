using AdaCredit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Data
{
    public static class AccountRepository
    {
        public static Account? GetAccountByBankAccountAndAgency(string bankAccount, string agency)
        {
            bankAccount = bankAccount.Insert(5, "-");
            var client = ClientRepository.GetClientByBankAccount(bankAccount);
            if (client != null)
                return client.Account;
            return null;
        }
    }
}
