using Nityo.DigitalAccount.Domain.Context.Commands;
using Nityo.DigitalAccount.Domain.Context.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nityo.DigitalAccount.Domain.Context.Interfaces
{
    public interface IDigitalAccountRepository
    {
        public bool ValidateAccountId(int AccountId);
        GetDigitalAccountBalanceQueryResult GetDigitalAccountBalance(DigitalAccountCommand AccountCommand);

        GetTransactionsQueryResult GetDigitalAccountExtract(DigitalAccountCommand AccountCommand);

        bool ProcessTransaction(TransactionCommand TransactionCommand);
    }
}
