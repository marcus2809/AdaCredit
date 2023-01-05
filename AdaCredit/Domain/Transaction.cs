using AdaCredit.Data;

namespace AdaCredit.Domain
{
    public enum TransactionType
    {
        DOC,
        TED,
        TEF
    }
    public sealed class Transaction
    {
        public string OriginBankCode { get; init; }
        public string OriginAgencyCode { get; init; }
        public string OriginBankAccount { get; init; }
        public string DestinationBankCode { get; init; }
        public string DestinationAgencyCode { get; init; }
        public string DestinationBankAccount { get; init; }
        public TransactionType TypeTransaction { get; init; }
        public decimal Value { get; init; }
        public string Details { get; set; }

        public Transaction() {}

        public override string ToString()
        {
            return $"{OriginBankAccount}, {DestinationBankAccount}, {TypeTransaction}, {Value}{(Details != null ? $", {Details}" : "")}";
        }

        public bool ProcessTransaction()
        {
            Account? account;

            if (OriginBankCode == "777" && DestinationBankCode == "777")
            {
                account = AccountRepository.GetAccountByBankAccountAndAgency(DestinationBankAccount, DestinationAgencyCode);
                Account? originAccount = AccountRepository.GetAccountByBankAccountAndAgency(OriginBankAccount, OriginAgencyCode);
                
                if (account == null)
                {
                    Details = "Conta de destino não existe";
                    return false;
                }
                if (originAccount == null)
                {
                    Details = "Conta de origem não existe";
                    return false;
                }
                if (TypeTransaction == TransactionType.DOC) 
                {
                    bool debt = DebtAccountDoc(originAccount);
                    if (!debt)
                        return false;
                    return CreditAccountDoc(account);
                }
                    

                if (TypeTransaction == TransactionType.TED)
                {
                    bool debt = DebtAccountTed(originAccount);
                    if (!debt)
                        return false;
                    return CreditAccountTed(account);
                }

                if (TypeTransaction == TransactionType.TEF)
                {
                    return CreditAccountTef(account, originAccount);
                }
            }

            else if(DestinationBankCode == "777")
            {
                account = AccountRepository.GetAccountByBankAccountAndAgency(DestinationBankAccount, DestinationAgencyCode);
                
                if(account == null)
                {
                    Details = "Conta de destino não existe";
                    return false;
                }
                if(TypeTransaction == TransactionType.DOC)
                    return CreditAccountDoc(account);
                
                if(TypeTransaction==TransactionType.TED)
                    return CreditAccountTed(account);

                if (TypeTransaction == TransactionType.TEF)
                {
                    Details = "Transferência TEF só pode ser realizada entre contas do mesmo banco";
                    return false;
                }
            }

            else if (OriginBankCode == "777")
            {
                account = AccountRepository.GetAccountByBankAccountAndAgency(DestinationBankAccount, OriginAgencyCode);

                if (account == null)
                {
                    Details = "Conta de origem não existe";
                    return false;
                }
                if (TypeTransaction == TransactionType.DOC)
                    return DebtAccountDoc(account);

                if (TypeTransaction == TransactionType.TED)
                    return DebtAccountTed(account);

                if (TypeTransaction == TransactionType.TEF)
                {
                    Details = "Transferência TEF só pode ser realizada entre contas do mesmo banco";
                    return false;
                }
            }

            Details = "Nenhuma das contas pertecem à AdaCredit";
            return false;
        }

        //Credit in destinationAccount and debt in originAccount
        private bool CreditAccountTef(Account destinationAccount, Account originAccount)
        {
            bool debt;
            debt = DebtAccount(originAccount, 0.0M);
            if (!debt)
                return false;
            CreditAccount(destinationAccount);

            return true;
        }

        private bool DebtAccountDoc(Account account)
        {
            decimal fee = 0.01M * Value;

            if (fee > 5.0M) 
                fee = 5.0M;
            fee += 1.0M;

            return DebtAccount(account, fee);
        }

        private bool CreditAccountDoc(Account account)
        {
            return CreditAccount(account);
        }

        private bool DebtAccountTed(Account account)
        {
            decimal fee = 5.0M;
            return DebtAccount(account, fee);
        }

        private bool CreditAccountTed(Account account)
        {
            return CreditAccount(account);
        }

        private bool DebtAccount(Account account, decimal fee)
        {
            decimal discount = Value + fee;
            if (account.Balance - discount < 0.0M)
            {
                Details = "Saldo insuficiente";
                Console.WriteLine(Details);
                return false;
            }
            account.Balance -= discount;
            return true;
        }

        private bool CreditAccount(Account account)
        {
            account.Balance += Value;
            return true;
        }
    }
}