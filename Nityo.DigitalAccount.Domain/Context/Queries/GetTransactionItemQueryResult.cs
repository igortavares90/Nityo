using System;
using System.Collections.Generic;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Queries
{
    public class GetTransactionItemQueryResult
    {
        public int TransactionId { get; set; }
        public int DigitalAccountId { get; set; }
        public string TransactionTypeDescription { get; set; }
        public decimal TransactionValue { get; set; }
        public decimal BalanceAfterTransaction { get; set; }

    }
}
