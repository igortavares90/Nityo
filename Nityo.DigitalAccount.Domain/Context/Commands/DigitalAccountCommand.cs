using FluentValidation;
using FluentValidation.Results;
using FluentValidator;
using Nityo.DigitalAccount.Domain.Context.Interfaces;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Nityo.DigitalAccount.Domain.Context.Commands
{
    public class DigitalAccountCommand : Notifiable, ICommand
    {
        public int AccountId { get; set; }

        public DateTime Day { get; set; }

        private ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            ValidationResult = new DigitalAccountValidation().Validate(this);

            foreach (var Error in this.ValidationResult.Errors)
                AddNotification(Error.PropertyName, Error.ErrorMessage);

            return ValidationResult.IsValid;
        }
    }

    public class DigitalAccountValidation : AbstractValidator<DigitalAccountCommand>
    {
        public DigitalAccountValidation()
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

            RuleFor(x => x.AccountId).GreaterThan(0).WithMessage("Id da conta inválido!");
        }
    }
}