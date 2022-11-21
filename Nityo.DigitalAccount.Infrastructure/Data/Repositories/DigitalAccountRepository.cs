using Dapper;
using Nityo.DigitalAccount.Domain.Context.Commands;
using Nityo.DigitalAccount.Domain.Context.Enums;
using Nityo.DigitalAccount.Domain.Context.Interfaces;
using Nityo.DigitalAccount.Domain.Context.Queries;
using System;
using System.Data;
using System.Linq;
using System.Text;

namespace Nityo.DigitalAccount.Infrastructure.Data.Repositories
{
    public class DigitalAccountRepository : IDigitalAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public DigitalAccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool ValidateAccountId(int AccountId)
        {
            var Query = $@"SELECT DA.AccountId AS DigitalAccountId
                           FROM DigitalAccount DA
                           WHERE DA.AccountId = @AccountId";

            var Entity = _unitOfWork.Connection.Query<GetDigitalAccountBalanceQueryResult>(Query, new { AccountId = AccountId }, commandType: CommandType.Text);

            var Account = Entity.Count();

            if (Account == 0)
            {
                return false;
            }

            return true;
        }

        public GetDigitalAccountBalanceQueryResult GetDigitalAccountBalance(DigitalAccountCommand AccountCommand)
        {
            var query = $@"SELECT DA.AccountId AS DigitalAccountId,
                                  DA.AccountBalance  
                           FROM DigitalAccount DA
                           WHERE DA.AccountId = @AccountId";

            var entity = _unitOfWork.Connection.Query<GetDigitalAccountBalanceQueryResult>(query, new { AccountId = AccountCommand.AccountId }, commandType: CommandType.Text);

            return entity.FirstOrDefault();
        }

        public GetTransactionsQueryResult GetDigitalAccountExtract(DigitalAccountCommand AccountCommand)
        {
            var InitialDate = AccountCommand.Day;
            var FinalDate = AccountCommand.Day.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = $@"SELECT DAT.Id AS TransactionId,
                                  DAT.DigitalAccountId,
                                  PMT.description AS TransactionTypeDescription, 
                                  DAT.Value AS TransactionValue,
                                  DAT.BalanceAfterTransaction
                                  ,(SELECT SUM(datr.Value) FROM DigitalAccountTransaction datr WHERE datr.Id = datr.Id AND datr.TransactionTypeId = 1) AS TotalDebts
                                  ,(SELECT SUM(datr.Value) FROM DigitalAccountTransaction datr WHERE datr.Id = datr.Id AND datr.TransactionTypeId = 2) AS TotalCredits
                           FROM DigitalAccountTransaction DAT
                           INNER JOIN TransactionType PMT ON PMT.id = DAT.TransactionTypeId 
                           WHERE DAT.DigitalAccountId = @AccountId";

            if(AccountCommand.Day != DateTime.MinValue)
            {
                query = query + " AND transactionDate BETWEEN @initialDate AND @finalDate";
            }

            var entity = _unitOfWork.Connection.Query<GetTransactionItemQueryResult>(query, new { AccountId = AccountCommand.AccountId, initialDate = InitialDate, finalDate = FinalDate }, commandType: CommandType.Text);

            var result = new GetTransactionsQueryResult();

            result.Items = entity.ToList();
            result.Total = result.Items.Count();

            var TotalsAccount = GetDigitalAccountDayBalance(AccountCommand);

            result.TotalDebts = TotalsAccount.TotalDebts;
            result.TotalCredits = TotalsAccount.TotalCredits;
            result.DailyBalance = TotalsAccount.TotalCredits - TotalsAccount.TotalDebts;

            return result;
        }

        public GetTransactionsQueryResult GetDigitalAccountDayBalance(DigitalAccountCommand AccountCommand)
        {
            var InitialDate = AccountCommand.Day;
            var FinalDate = AccountCommand.Day.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = $@"SELECT (SELECT SUM(datr.Value) FROM DigitalAccountTransaction datr WHERE digitalaccountId = @AccountId AND datr.TransactionTypeId = 1 AND transactionDate BETWEEN @initialDate AND @finalDate) AS TotalCredits
                         ,(SELECT SUM(datr.Value) FROM DigitalAccountTransaction datr WHERE digitalaccountId = @AccountId AND datr.Id = datr.Id AND datr.TransactionTypeId = 2 AND transactionDate BETWEEN @initialDate AND @finalDate) AS TotalDebts";

            var entity = _unitOfWork.Connection.Query<GetTransactionsQueryResult>(query, new { AccountId = AccountCommand.AccountId, initialDate = InitialDate, finalDate = FinalDate }, commandType: CommandType.Text);

            return entity.FirstOrDefault();
        }

        public bool ProcessTransaction(TransactionCommand TransactionCommand)
        {
            try
            {
                var query = new StringBuilder();
                query.Append("UPDATE DIGITALACCOUNT SET AccountBalance=AccountBalance");

                query.Append(TransactionCommand.TransactionTypeId == ETransactionType.Credit
                                                                        ? "+ @TransactionValue"
                                                                        : "- @TransactionValue");
                query.Append(",UpdateDate = NOW() ");
                query.Append(" WHERE AccountId = @DigitalAccountId; ");
                query.Append("SELECT AccountId FROM DigitalAccount WHERE AccountId = @DigitalAccountId ; ");

                var id = _unitOfWork.Connection.Query<long>(query.ToString(), TransactionCommand, commandType: CommandType.Text).FirstOrDefault();

                if (id == 0)
                {
                    return false;
                }

                query = new StringBuilder();
                query.Append("SELECT AccountBalance FROM DigitalAccount WHERE AccountId = @DigitalAccountId;");
                var BalanceAfterTransaction = _unitOfWork.Connection.Query<decimal>(query.ToString(), TransactionCommand, commandType: CommandType.Text).FirstOrDefault();

                query = new StringBuilder();
                query.Append("INSERT INTO DigitalAccountTransaction(DigitalAccountId, Value,BalanceAfterTransaction,TransactionTypeId, TransactionDate)");
                query.Append("VALUES");
                query.Append($"({id}, @TransactionValue, {BalanceAfterTransaction.ToString().Replace(",", ".")}, @TransactionTypeId, NOW());");

                _unitOfWork.Connection.Query(query.ToString(), TransactionCommand, commandType: CommandType.Text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
