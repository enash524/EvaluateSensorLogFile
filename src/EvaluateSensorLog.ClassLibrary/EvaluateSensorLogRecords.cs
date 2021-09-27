using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Commands.ParseSensorRecordFile;
using EvaluateSensorLog.Application.Commands.ValidateSensorRecord;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using EvaluateSensorLog.Common.Extensions;
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

            if (sensorLogModel.CommandResultType != CommandResultType.Success)
            {
                _logger.LogError("Send Sensor Log Command Async with Path: {path} produced errors and returned status: {status}", path, sensorLogModel.CommandResultType);
                return null;
            }

            CommandResult<ValidateSensorLogModel> validateSensorLogModel = await SendValidateSensorRecordCommandAsync(sensorLogModel.Result);

            if (validateSensorLogModel.CommandResultType != CommandResultType.Success)
            {
                _logger.LogError("Send Validate Sensor Record Command Async with Path: {path} produced errors and returned status: {status}", path, validateSensorLogModel.CommandResultType);
                return null;
            }

            string result = GenerateOutput(validateSensorLogModel.Result);

            return result;
        }

        /// <summary>
        /// Generates a JSON string representing the sensor log quality control evaluation
        /// </summary>
        /// <param name="validateSensorLogModel">The validated sensor log model</param>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        private string GenerateOutput(ValidateSensorLogModel validateSensorLogModel)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            foreach (ThermometerResultModel thermometerResultModel in validateSensorLogModel.ThermometerResults)
            {
                results.Add(thermometerResultModel.SensorName, thermometerResultModel.ThermometerStatus.GetDescription().ToLower());
            }

            foreach (HumidityResultModel humidityResultModel in validateSensorLogModel.HumidityResults)
            {
                results.Add(humidityResultModel.SensorName, humidityResultModel.HumidityStatus.GetDescription().ToLower());
            }

            foreach (MonoxideResultModel monoxideResultModel in validateSensorLogModel.MonoxideResults)
            {
                results.Add(monoxideResultModel.SensorName, monoxideResultModel.MonoxideStatus.GetDescription().ToLower());
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(results, options);

            return jsonString;
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
        private Task<CommandResult<ValidateSensorLogModel>> SendValidateSensorRecordCommandAsync(SensorLogModel sensorLogModel)
        {
            ValidateSensorRecordCommand command = new ValidateSensorRecordCommand
            {
                SensorLogModel = sensorLogModel
            };

            return _mediator.Send(command); ;
        }
    }
}
