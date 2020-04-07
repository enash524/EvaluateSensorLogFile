using System;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using EvaluateSensorLog.ClassLibrary.Models;
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
        private readonly IParseSensorRecordFile _parseSensorRecordFile;
        private readonly IValidateSensorRecord _validateSensorRecord;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateSensorLogRecords`1"/> class.
        /// </summary>
        /// <param name="logger">DI injected logger</param>/param>
        public EvaluateSensorLogRecords(ILogger<EvaluateSensorLogRecords> logger, IParseSensorRecordFile parseSensorRecordFile, IValidateSensorRecord validateSensorRecord)
        {
            _logger = logger;
            _parseSensorRecordFile = parseSensorRecordFile;
            _validateSensorRecord = validateSensorRecord;
        }

        /// <summary>
        /// Reads sensor log input text file and outputs sensor log report
        /// </summary>
        /// <param name="logContentsStr">The sensor log input text file</param>
        /// <exception cref="ArgumentException">logContentsStr is null, empty or whitespace</exception>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        public string EvaluateLogFile(string logContentsStr)
        {
            if (string.IsNullOrWhiteSpace(logContentsStr))
            {
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(logContentsStr)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(logContentsStr));
            }

            SensorLogModel sensorLogModel = _parseSensorRecordFile.ParseInputLogFile(logContentsStr);
            string result = _validateSensorRecord.ValidateSensorLogRecords(sensorLogModel);

            return result;
        }
    }
}