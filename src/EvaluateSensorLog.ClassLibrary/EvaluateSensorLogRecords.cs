using System;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Commands.ParseSensorRecordFile;
using EvaluateSensorLog.Application.Commands.ValidateSensorRecord;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using EvaluateSensorLog.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.ClassLibrary
{
    /// <summary>
    /// Contains methods for parsing and validating sensor reading records
    /// </summary>
    /// <seealso cref="IEvaluateSensorLogRecords"/>
    public class EvaluateSensorLogRecords : IEvaluateSensorLogRecords
    {
        private readonly ILogger<EvaluateSensorLogRecords> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateSensorLogRecords`1"/> class.
        /// </summary>
        /// <param name="logger">DI injected logger</param>
        public EvaluateSensorLogRecords(
            ILogger<EvaluateSensorLogRecords> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Reads sensor log input text file and outputs sensor log report
        /// </summary>
        /// <param name="logContentsStr">The sensor log input text file</param>
        /// <exception cref="ArgumentException">logContentsStr is null, empty or whitespace</exception>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        public async Task<string> EvaluateLogFileAsync(string path)
        {
            CommandResult<SensorLogModel> sensorLogModel = await SendSensorLogCommandAsync(path);

            // TODO - VALIDATE COMMAND RESULT!!!

            CommandResult<string> result = await SendValidateSensorRecordCommandAsync(sensorLogModel.Result);

            // TODO - VALIDATE SENSOR RECORD VALIDATION RESULT!!!

            return result.Result;
        }

        /// <summary>
        /// Asynchronously sends command to handler
        /// </summary>
        /// <param name="path">The path to the sensor log record to be parsed</param>
        /// <returns>The result of the command</returns>
        private Task<CommandResult<SensorLogModel>> SendSensorLogCommandAsync(string path)
        {
            ParseSensorRecordCommand command = new ParseSensorRecordCommand
            {
                Path = path
            };

            return _mediator.Send(command);
        }

        /// <summary>
        /// Asynchronously sends command to handler
        /// </summary>
        /// <param name="sensorLogModel">The sensor log model to validate</param>
        /// <returns>The result of the command</returns>
        private Task<CommandResult<string>> SendValidateSensorRecordCommandAsync(SensorLogModel sensorLogModel)
        {
            ValidateSensorRecordCommand command = new ValidateSensorRecordCommand
            {
                SensorLogModel = sensorLogModel
            };

            return _mediator.Send(command); ;
        }
    }
}