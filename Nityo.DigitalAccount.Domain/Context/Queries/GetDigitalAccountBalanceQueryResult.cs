using System;
using System.Collections.Generic;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Queries
{
    public class GetDigitalAccountBalanceQueryResult
    {
        public int DigitalAccountId { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
