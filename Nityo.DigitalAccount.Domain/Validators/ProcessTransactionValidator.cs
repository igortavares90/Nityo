using FluentValidation;
using Nityo.DigitalAccount.Domain.Context.Commands;

namespace Nityo.DigitalAccount.Domain.Validators
{
    public class ProcessTransactionValidator : AbstractValidator<TransactionCommand>
    {
        public ProcessTransactionValidator()
        {
            RuleFor(x => x.DigitalAccountId)
                .NotEmpty().WithMessage("Informe o Id da conta.")
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");

            RuleFor(x => x.TransactionValue)
                .NotEmpty().WithMessage("Informe o valor da transação")
                .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");

            RuleFor(x => x.TransactionTypeId)
                .NotEmpty().WithMessage("Informe o tipo da transação.");

        }
    }
}
