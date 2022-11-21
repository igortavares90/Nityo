using Nityo.DigitalAccount.Domain.Context.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Commands
{
    public class TransactionCommand
    {
        public int DigitalAccountId { get; set; }
        /// <summary>
        /// 2 = Débito
        /// </summary>
        public decimal TransactionValue { get; set; }
        public ETransactionType TransactionTypeId { get; set; }
    }
}
