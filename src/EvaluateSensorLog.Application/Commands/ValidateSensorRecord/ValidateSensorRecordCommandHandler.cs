using System.Threading;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.Data.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.Application.Commands.ValidateSensorRecord
{
    /// <summary>
    /// Handles the ValidateSensorRecordCommand
    /// </summary>
    public class ValidateSensorRecordCommandHandler : IRequestHandler<ValidateSensorRecordCommand, CommandResult<string>>
    {
        private readonly ILogger<ValidateSensorRecordCommandHandler> _logger;
        private readonly IValidateSensorRecordRepository _repository;
        private readonly IValidator<ValidateSensorRecordCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSensorRecordCommandHandler`1"/> class
        /// </summary>
        /// <param name="logger">Logger</param>
        public ValidateSensorRecordCommandHandler(
            ILogger<ValidateSensorRecordCommandHandler> logger,
            IValidateSensorRecordRepository repository,
            IValidator<ValidateSensorRecordCommand> validator)
        {
            _logger = logger;
            _repository = repository;
            _validator = validator;
        }

        /// <summary>
        /// Handles the ValidateSensorRecordCommand request
        /// </summary>
        /// <param name="command">ValidateSensorRecordCommand request parameters</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.</param>
        /// <returns>Returns a JSON string validation record wrapped in a CommandResult object</returns>
        public Task<CommandResult<string>> Handle(ValidateSensorRecordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationResult validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.LogError("Validate Sensor Record produced errors {errors}", validation.ToString());

                CommandResult<string> invalidCommandResult = new CommandResult<string>()
                {
                    Result = null,
                    CommandResultType = CommandResultType.InvalidInput
                };

                return Task.FromResult(invalidCommandResult);
            }

            string result = _repository.ValidateSensorLogRecords(command.SensorLogModel);

            if (string.IsNullOrWhiteSpace(result))
            {
                _logger.LogError("Validate Sensor Record produced an invalid result");

                CommandResult<string> errorCommandResult = new CommandResult<string>()
                {
                    Result = null,
                    CommandResultType = CommandResultType.Error
                };

                return Task.FromResult(errorCommandResult);
            }

            CommandResult<string> commandResult = new CommandResult<string>()
            {
                Result = result,
                CommandResultType = CommandResultType.Success
            };

            return Task.FromResult(commandResult);
        }
    }
}