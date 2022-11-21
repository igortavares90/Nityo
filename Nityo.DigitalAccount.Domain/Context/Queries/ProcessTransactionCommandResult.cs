using System;
using System.Collections.Generic;
using System.Text;

namespace Nityo.DigitalAccount.Domain.Context.Queries
{
    public class ProcessTransactionCommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ProcessTransactionCommandResult()
        {
            Success = true;
            Message = "Transação processada com sucesso!";
        }
    }
}
