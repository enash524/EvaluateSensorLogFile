using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary
{
    /// <summary>
    /// Contains methods for parsing and validating sensor reading records
    /// </summary>
    /// <seealso cref="IEvaluateSensorLogRecords"/>
    public class EvaluateSensorLogRecords : IEvaluateSensorLogRecords
    {
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
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(logContentsStr));
            }

            SensorLogModel sensorLogModel = EvaluateInputLogFile(logContentsStr);
            string result = ValidateSensorLogRecords(sensorLogModel);

            return result;
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
        /// Generates a sensor log model from the sensor log input text file
        /// </summary>
        /// <param name="logContentsStr">The sensor log input text file</param>
        /// <exception cref="ArgumentException">logContentStr is null or whitespace</exception>
        /// <exception cref="NotImplementedException">Invalid record type is entered</exception>
        /// <returns>The sensor log model representing the sensors and reading log records</returns>
        private SensorLogModel EvaluateInputLogFile(string logContentsStr)
        {
            if (string.IsNullOrWhiteSpace(logContentsStr))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(logContentsStr));
            }

            SensorLogModel model = new SensorLogModel();
            string[] input = logContentsStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Regex timeValueRegex = new Regex("[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])T(2[0-3]|[01][0-9]):[0-5][0-9]");

            for (int i = 0; i < input.Length; i++)
            {
                string[] line = input[i].Split(' ');

                switch (line[0])
                {
                    case "reference":
                        model.ReferenceValues = HandleReferenceValuesLogRecord(line[1], line[2], line[3]);
                        break;

                    case "thermometer":
                        ThermometerModel thermometerModel = new ThermometerModel
                        {
                            Name = line[1]
                        };

                        while (true)
                        {
                            i++;
                            line = input[i].Split(' ');

                            if (timeValueRegex.IsMatch(line[0]))
                            {
                                DecimalReadingModel readingModel = HandleDecimalReading(line[0], line[1]);
                                thermometerModel.Readings.Add(readingModel);
                            }
                            else
                            {
                                i--;
                                break;
                            }

                            if (i + 1 >= input.Length)
                            {
                                break;
                            }
                        }

                        model.ThermometerReadings.Add(thermometerModel);
                        break;

                    case "humidity":
                        HumidityModel humidityModel = new HumidityModel
                        {
                            Name = line[1]
                        };

                        while (true)
                        {
                            i++;
                            line = input[i].Split(' ');

                            if (timeValueRegex.IsMatch(line[0]))
                            {
                                DecimalReadingModel readingModel = HandleDecimalReading(line[0], line[1]);
                                humidityModel.Readings.Add(readingModel);
                            }
                            else
                            {
                                i--;
                                break;
                            }

                            if (i + 1 >= input.Length)
                            {
                                break;
                            }
                        }

                        model.HumidityReadings.Add(humidityModel);
                        break;

                    case "monoxide":
                        MonoxideModel monoxideModel = new MonoxideModel
                        {
                            Name = line[1]
                        };

                        while (true)
                        {
                            i++;
                            line = input[i].Split(' ');

                            if (timeValueRegex.IsMatch(line[0]))
                            {
                                IntegerReadingModel readingModel = HandleIntegerReading(line[0], line[1]);
                                monoxideModel.Readings.Add(readingModel);
                            }
                            else
                            {
                                i--;
                                break;
                            }

                            if (i + 1 >= input.Length)
                            {
                                break;
                            }
                        }

                        model.MonoxideReadings.Add(monoxideModel);
                        break;

                    default:
                        throw new NotImplementedException($"Sensor type {line[0]} has not been implemented yet.");
                }
            }

            return model;
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
        /// Reads the sensors and the log records and generates the quality control evaluation for the sensors
        /// </summary>
        /// <param name="sensorLogModel">The sensor log input model</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A JSON string representing the sensor log quality control evaluation</returns>
        private string ValidateSensorLogRecords(SensorLogModel sensorLogModel)
        {
            if (sensorLogModel == null)
            {
                throw new ArgumentException(Messages.NullValue, nameof(sensorLogModel));
            }

            if (sensorLogModel.ReferenceValues == null)
            {
                throw new ArgumentException(Messages.NullValue, nameof(sensorLogModel.ReferenceValues));
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

        /// <summary>
        /// Parses the raw text input and generates a decimal sensor log record
        /// </summary>
        /// <param name="timestampInputValue">The timestamp string value from the input log record</param>
        /// <param name="decimalInputValue">The decimal string value from the input log record</param>
        /// <exception cref="ArgumentException">timestampInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">decimalInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">timestampInputValue cannot be parsed to a DateTime</exception>
        /// <exception cref="ArgumentException">decimalInputValue cannot be parsed to a decimal</exception>
        /// <returns>Integer sensor log record</returns>
        private DecimalReadingModel HandleDecimalReading(string timestampInputValue, string decimalInputValue)
        {
            if (string.IsNullOrWhiteSpace(timestampInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(timestampInputValue));
            }

            if (string.IsNullOrWhiteSpace(decimalInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(decimalInputValue));
            }

            if (!DateTime.TryParse(timestampInputValue, out DateTime timestamp))
            {
                throw new ArgumentException(Messages.InvalidDateTimeValue, nameof(timestampInputValue));
            }

            if (!decimal.TryParse(decimalInputValue, out decimal value))
            {
                throw new ArgumentException(Messages.InvalidDecimalValue, nameof(decimalInputValue));
            }

            DecimalReadingModel readingModel = new DecimalReadingModel
            {
                Timestamp = timestamp,
                Value = value
            };

            return readingModel;
        }

        /// <summary>
        /// Parses the raw text input and generates an integer sensor log record
        /// </summary>
        /// <param name="timestampInputValue">The timestamp string value from the input log record</param>
        /// <param name="integerInputValue">The integer string value from the input log record</param>
        /// <exception cref="ArgumentException">timestampInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">integerInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">timestampInputValue cannot be parsed to a DateTime</exception>
        /// <exception cref="ArgumentException">integerInputValue cannot be parsed to an integer</exception>
        /// <returns>Integer sensor log record</returns>
        private IntegerReadingModel HandleIntegerReading(string timestampInputValue, string integerInputValue)
        {
            if (string.IsNullOrWhiteSpace(timestampInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(timestampInputValue));
            }

            if (string.IsNullOrWhiteSpace(integerInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(integerInputValue));
            }

            if (!DateTime.TryParse(timestampInputValue, out DateTime timestamp))
            {
                throw new ArgumentException(Messages.InvalidDateTimeValue, nameof(timestampInputValue));
            }

            if (!int.TryParse(integerInputValue, out int value))
            {
                throw new ArgumentException(Messages.InvalidIntegerValue, nameof(integerInputValue));
            }

            IntegerReadingModel readingModel = new IntegerReadingModel
            {
                Timestamp = timestamp,
                Value = value
            };

            return readingModel;
        }

        /// <summary>
        /// Parses the raw text input and generates the reference values model
        /// </summary>
        /// <param name="line">The raw reference values input</param>
        /// <exception cref="ArgumentException">thermometerInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">humidityInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">monoxideInputValue is null or whitespace</exception>
        /// <exception cref="ArgumentException">thermometerInputValue cannot be parsed to a decimal</exception>
        /// <exception cref="ArgumentException">humidityInputValue cannot be parsed to a decimal</exception>
        /// <exception cref="ArgumentException">monoxideInputValue cannot be parsed to an integer</exception>
        /// <returns>Reference values model</returns>
        private ReferenceValuesModel HandleReferenceValuesLogRecord(string thermometerInputValue, string humidityInputValue, string monoxideInputValue)
        {
            if (string.IsNullOrWhiteSpace(thermometerInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(thermometerInputValue));
            }

            if (string.IsNullOrWhiteSpace(humidityInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(humidityInputValue));
            }

            if (string.IsNullOrWhiteSpace(monoxideInputValue))
            {
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(monoxideInputValue));
            }

            if (!decimal.TryParse(thermometerInputValue, out decimal thermometerValue))
            {
                throw new ArgumentException(Messages.InvalidDecimalValue, nameof(thermometerInputValue));
            }

            if (!decimal.TryParse(humidityInputValue, out decimal humidityValue))
            {
                throw new ArgumentException(Messages.InvalidDecimalValue, nameof(humidityInputValue));
            }

            if (!int.TryParse(monoxideInputValue, out int monoxideValue))
            {
                throw new ArgumentException(Messages.InvalidIntegerValue, nameof(monoxideInputValue));
            }

            ReferenceValuesModel model = new ReferenceValuesModel
            {
                ThermometerReferenceValue = thermometerValue,
                HumidityReferenceValue = humidityValue,
                MonoxideReferenceValue = monoxideValue
            };

            return model;
        }
    }
}