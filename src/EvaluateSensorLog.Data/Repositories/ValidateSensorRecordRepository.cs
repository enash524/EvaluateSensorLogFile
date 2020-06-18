using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.Data.Repositories
{
    /// <summary>
    /// Contains methods for validating sensor records
    /// </summary>
    public class ValidateSensorRecordRepository : IValidateSensorRecordRepository
    {
        /// <summary>
        /// DI injected logger
        /// </summary>
        private readonly ILogger<ValidateSensorRecordRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSensorRecordRepository`1"/> class.
        /// </summary>
        /// <param name="logger">DI injected logger</param>
        public ValidateSensorRecordRepository(ILogger<ValidateSensorRecordRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Reads the sensors and the log records and generates the quality control evaluation for the sensors
        /// </summary>
        /// <param name="sensorLogModel">The sensor log input model</param>
        /// <exception cref="ArgumentNullException">sensorLogModel is null or sensorLogModel.ReverenceValues is null</exception>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        public string ValidateSensorLogRecords(SensorLogModel sensorLogModel)
        {
            if (sensorLogModel == null)
            {
                _logger.LogError($"{Messages.NullValue} (Parameter '{nameof(sensorLogModel)}')");
                throw new ArgumentNullException(nameof(sensorLogModel), Messages.NullValue);
            }

            if (sensorLogModel.ReferenceValues == null)
            {
                _logger.LogError($"{Messages.NullValue} (Parameter '{nameof(sensorLogModel.ReferenceValues)}')");
                throw new ArgumentNullException(nameof(sensorLogModel.ReferenceValues), Messages.NullValue);
            }

            Dictionary<string, string> results = new Dictionary<string, string>();
            List<KeyValuePair<string, string>> thermometerResult = EvaluateThermometerLogRecords(sensorLogModel.ThermometerReadings, sensorLogModel.ReferenceValues.ThermometerReferenceValue);
            List<KeyValuePair<string, string>> humidityResult = EvaluateHumidityLogRecords(sensorLogModel.HumidityReadings, sensorLogModel.ReferenceValues.HumidityReferenceValue);
            List<KeyValuePair<string, string>> monoxideResult = EvaluateMonoxideLogRecords(sensorLogModel.MonoxideReadings, sensorLogModel.ReferenceValues.MonoxideReferenceValue);
            List<KeyValuePair<string, string>> recordResults = new List<KeyValuePair<string, string>>();

            recordResults.AddRange(thermometerResult);
            recordResults.AddRange(humidityResult);
            recordResults.AddRange(monoxideResult);

            foreach (KeyValuePair<string, string> kvp in recordResults)
            {
                results.Add(kvp.Key, kvp.Value);
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(results, options);

            return jsonString;
        }

        /// <summary>
        /// Computes the standard deviation of the decimal reading values
        /// </summary>
        /// <param name="readings">List of the decimal reading values</param>
        /// <exception cref="ArgumentException">readings collection is null or empty</exception>
        /// <returns>The standard deviation of the decimal reading values</returns>
        private decimal ComputeStandardDeviation(List<DecimalReadingModel> readings)
        {
            if (readings == null || readings.Count == 0)
            {
                _logger.LogError($"{Messages.NullEmptyCollection} (Parameter '{nameof(readings)}')");
                throw new ArgumentException(Messages.NullEmptyCollection, nameof(readings));
            }

            decimal mean = readings.Average(r => r.Value);
            decimal squares = readings.Select(r => (r.Value - mean) * (r.Value - mean)).Sum();
            decimal result = (decimal)Math.Sqrt((double)squares / readings.Count);

            return result;
        }

        /// <summary>
        /// Evaluates the list of humidity sensor log records and generates the quality control evaluation
        /// </summary>
        /// <param name="humidityModel">List of the humidity sensors and their reading log records</param>
        /// <param name="referenceValue">The humidity reference value</param>
        /// <exception cref="ArgumentException">humidityModel collection is null or empty</exception>
        /// <returns>A list of key value pairs which represents the sensor name and quality control evaluation</returns>
        private List<KeyValuePair<string, string>> EvaluateHumidityLogRecords(List<HumidityModel> humidityModel, decimal referenceValue)
        {
            if (humidityModel == null || humidityModel.Count == 0)
            {
                _logger.LogError($"{Messages.NullEmptyCollection} (Parameter '{nameof(humidityModel)}')");
                throw new ArgumentException(Messages.NullEmptyCollection, nameof(humidityModel));
            }

            List<KeyValuePair<string, string>> kvp = new List<KeyValuePair<string, string>>();

            foreach (HumidityModel model in humidityModel)
            {
                bool passed = model.Readings.All(h => referenceValue - 1m <= h.Value && h.Value <= referenceValue + 1m);
                string status = passed ? "keep" : "discard";
                KeyValuePair<string, string> result = new KeyValuePair<string, string>(model.Name, status);

                kvp.Add(result);
            }

            return kvp;
        }

        /// <summary>
        /// Evaluates the list of monoxide sensor log records and generates the quality control evaluation
        /// </summary>
        /// <param name="monoxideModel">List of the monoxide sensors and their reading log records</param>
        /// <param name="referenceValue">The monoxide reference value</param>
        /// <exception cref="ArgumentException">monoxideModel collection is null or empty</exception>
        /// <returns>A list of key value pairs which represents the sensor name and quality control evaluation</returns>
        private List<KeyValuePair<string, string>> EvaluateMonoxideLogRecords(List<MonoxideModel> monoxideModel, int referenceValue)
        {
            if (monoxideModel == null || monoxideModel.Count == 0)
            {
                _logger.LogError($"{Messages.NullEmptyCollection} (Parameter '{nameof(monoxideModel)}')");
                throw new ArgumentException(Messages.NullEmptyCollection, nameof(monoxideModel));
            }

            List<KeyValuePair<string, string>> kvp = new List<KeyValuePair<string, string>>();

            foreach (MonoxideModel model in monoxideModel)
            {
                bool passed = model.Readings.All(r => referenceValue - 3 <= r.Value && r.Value <= referenceValue + 3);
                string status = passed ? "keep" : "discard";
                KeyValuePair<string, string> result = new KeyValuePair<string, string>(model.Name, status);

                kvp.Add(result);
            }

            return kvp;
        }

        /// <summary>
        /// Evaluates the list of thermometer sensor log records and generates the quality control evaluation
        /// </summary>
        /// <param name="thermometerModel">List of the thermometer sensors and their reading log records</param>
        /// <param name="referenceValue">Thermometer reference value</param>
        /// <exception cref="ArgumentException">thermometerModel collection is null or empty</exception>
        /// <returns>A list of key value pairs which represents the sensor name and quality control evaluation</returns>
        private List<KeyValuePair<string, string>> EvaluateThermometerLogRecords(List<ThermometerModel> thermometerModel, decimal referenceValue)
        {
            if (thermometerModel == null || thermometerModel.Count == 0)
            {
                _logger.LogError($"{Messages.NullEmptyCollection} (Parameter '{nameof(thermometerModel)}')");
                throw new ArgumentException(Messages.NullEmptyCollection, nameof(thermometerModel));
            }

            List<KeyValuePair<string, string>> kvp = new List<KeyValuePair<string, string>>();

            foreach (ThermometerModel model in thermometerModel)
            {
                decimal averageValue = Math.Round(model.Readings.Average(r => r.Value), 1);
                decimal standardDeviation = Math.Round(ComputeStandardDeviation(model.Readings), 1);
                string status = GenerateThermometerStatus(referenceValue, averageValue, standardDeviation);
                KeyValuePair<string, string> result = new KeyValuePair<string, string>(model.Name, status);

                kvp.Add(result);
            }

            return kvp;
        }

        /// <summary>
        /// Generates the thermometer status value from the inputs
        /// </summary>
        /// <param name="referenceValue">Thermometer reference value</param>
        /// <param name="averageValue">Thermometer log records average value</param>
        /// <param name="standardDeviation">Thermometer log records standard deviation</param>
        /// <returns>Thermometer status</returns>
        private string GenerateThermometerStatus(decimal referenceValue, decimal averageValue, decimal standardDeviation)
        {
            if (referenceValue - 0.5m <= averageValue && averageValue <= referenceValue + 0.5m)
            {
                if (standardDeviation < 3)
                {
                    return "ultra precise";
                }
                else if (standardDeviation < 5)
                {
                    return "very precise";
                }
            }

            return "precise";
        }
    }
}