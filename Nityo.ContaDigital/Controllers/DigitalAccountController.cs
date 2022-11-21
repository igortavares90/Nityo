using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nityo.DigitalAccount.Domain.Context.Commands;
using Nityo.DigitalAccount.Domain.Context.Interfaces;
using Nityo.DigitalAccount.Domain.Context.Queries;
using Nityo.DigitalAccount.Domain.Validators;
using System;
using System.Threading.Tasks;

namespace Nityo.ContaDigital.Controllers
{
    [ApiController]
    public class DigitalAccountController : ControllerBase
    {
        private readonly IDigitalAccountRepository _digitalAccountRepository;
        private readonly ProcessTransactionValidator _validator;

        public DigitalAccountController(IDigitalAccountRepository digitalAccountRepository, ProcessTransactionValidator validator)
        {
            _digitalAccountRepository = digitalAccountRepository;
            _validator = validator;
        }

        [HttpGet]
        [Route("GetDigitalAccountBalance")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetDigitalAccountBalanceQueryResult), StatusCodes.Status200OK)]
        public IActionResult GetDigitalAccountBalance([FromQuery] DigitalAccountCommand digitalAccountcommand)
        {
            try
            {
                if (!_digitalAccountRepository.ValidateAccountId(digitalAccountcommand.AccountId))
                {
                    return new NotFoundObjectResult($"Conta Id {digitalAccountcommand.AccountId} não encontrada!");
                }

                var result = _digitalAccountRepository.GetDigitalAccountBalance(digitalAccountcommand);

                if (result == null)
                    return new NotFoundObjectResult($"Conta Id {digitalAccountcommand.AccountId} não encontrada!");
                else
                    return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDigitalAccountDailyExtract")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetDigitalAccountBalanceQueryResult), StatusCodes.Status200OK)]
        public IActionResult GetDigitalAccountExtract([FromQuery] DigitalAccountCommand digitalAccountcommand)
        {
            try
            {
                if (!_digitalAccountRepository.ValidateAccountId(digitalAccountcommand.AccountId))
                {
                    return new NotFoundObjectResult($"Conta Id {digitalAccountcommand.AccountId} não encontrada!");
                }

                var result = _digitalAccountRepository.GetDigitalAccountExtract(digitalAccountcommand);

                if (result == null)
                    return new NotFoundObjectResult($"Conta Id {digitalAccountcommand.AccountId} não encontrada!");
                else
                    return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ProcessTransaction")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetDigitalAccountBalanceQueryResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProcessTransactionCommandResult>> ProcessTransaction([FromQuery] TransactionCommand transactionCommand)
        {
            try
            {
                if (!_digitalAccountRepository.ValidateAccountId(transactionCommand.DigitalAccountId))
                {
                    return new NotFoundObjectResult($"Conta Id {transactionCommand.DigitalAccountId} não encontrada!");
                }

                ValidationResult ResultValidation = await _validator.ValidateAsync(transactionCommand);

                if (!ResultValidation.IsValid)
                {
                    return new BadRequestObjectResult(ResultValidation);
                }

                var result = _digitalAccountRepository.ProcessTransaction(transactionCommand);

                if (!result)
                    return new ProcessTransactionCommandResult { Success = false, Message = "Erro ao atualizar a conta!" };
                else
                    return new ProcessTransactionCommandResult();
            }
            catch (Exception)
            {
                return new ProcessTransactionCommandResult { Success = false, Message = "Erro ao atualizar a conta!" };
            }
        }
    }
}
