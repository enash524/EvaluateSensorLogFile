using System.Threading;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.Application.Commands.ParseSensorRecordFile
{
    /// <summary>
    /// Handles the ParseSensorRecordCommand
    /// </summary>
    public class ParseSensorRecordCommandHandler : IRequestHandler<ParseSensorRecordCommand, CommandResult<SensorLogModel>>
    {
        private readonly ILogger<ParseSensorRecordCommandHandler> _logger;
        private readonly IParseSensorRecordRepository _repository;
        private readonly IValidator<ParseSensorRecordCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordCommandHandler`1"/> class
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="repository">ParseSensorRecordRepository</param>
        /// <param name="validator">FluentValidation validator of the input parameters</param>
        public ParseSensorRecordCommandHandler(
            ILogger<ParseSensorRecordCommandHandler> logger,
            IParseSensorRecordRepository repository,
            IValidator<ParseSensorRecordCommand> validator)
        {
            _logger = logger;
            _repository = repository;
            _validator = validator;
        }

        /// <summary>
        /// Handles the ParseSensorRecordCommand request
        /// </summary>
        /// <param name="command">ParseSensorRecordCommand request parameters</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is System.Threading.CancellationToken.None.</param>
        /// <returns>Returns a SensorLogModel wrapped in a CommandResult object</returns>
        public async Task<CommandResult<SensorLogModel>> Handle(ParseSensorRecordCommand command, CancellationToken cancellationToken = default)
        {
            ValidationResult validation = _validator.Validate(command);

            if (!validation.IsValid)
            {
                _logger.LogError("Parse Sensor Record Command with Path: {path} produced errors on validation: {errors}", command.Path, validation.ToString());

                return new CommandResult<SensorLogModel>
                {
                    Result = null,
                    CommandResultType = CommandResultType.InvalidInput
                };
            }

            SensorLogModel result = await _repository.ParseInputLogFileAsync(command.Path);

            if (result == null)
            {
                _logger.LogError("Parse Sensor Record Command with Path: {path} produced an invalid result.", command.Path);

                return new CommandResult<SensorLogModel>
                {
                    Result = null,
                    CommandResultType = CommandResultType.Error
                };
            }

            return new CommandResult<SensorLogModel>
            {
                Result = result,
                CommandResultType = CommandResultType.Success
            };
        }
    }
}
