using System;
using System.Collections.Generic;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Queries
{
    public class GetTransactionsQueryResult
    {
        public int Total { get; set; }

        public decimal TotalDebts { get; set; }

        public decimal TotalCredits { get; set; }

        public decimal DailyBalance { get; set; }

        public List<GetTransactionItemQueryResult> Items { get; set; }
    }
}
