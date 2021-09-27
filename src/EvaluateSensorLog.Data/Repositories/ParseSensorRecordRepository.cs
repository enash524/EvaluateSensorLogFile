using System;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EvaluateSensorLog.Data.Repositories
{
    /// <summary>
    /// Contains methods for parsing a sensor log record file
    /// </summary>
    public class ParseSensorRecordRepository : IParseSensorRecordRepository
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// DI injected logger
        /// </summary>
        private readonly ILogger<ParseSensorRecordRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordRepository`1"/> class.
        /// </summary>
        /// <param name="logger">DI injected logger</param>
        public ParseSensorRecordRepository(IFileSystem fileSystem, ILogger<ParseSensorRecordRepository> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        /// <summary>
        /// Parses an input file and returns the resulting sensor log model
        /// </summary>
        /// <param name="path">The path to the sensor log record to parse</param>
        /// <returns>A task representing the resulting sensor log model</returns>
        public async Task<SensorLogModel> ParseInputLogFileAsync(string path)
        {
            string input = await _fileSystem.File.ReadAllTextAsync(path);
            SensorLogModel model = ParseInputLogFile(input);

            return model;
        }

        /// <summary>
        /// Generates a sensor log model from the sensor log input text file
        /// </summary>
        /// <param name="logContentsStr">The sensor log input text file</param>
        /// <exception cref="ArgumentException">logContentStr is null or whitespace</exception>
        /// <exception cref="NotImplementedException">Invalid record type is entered</exception>
        /// <returns>The sensor log model representing the sensors and reading log records</returns>
        private SensorLogModel ParseInputLogFile(string logContentsStr)
        {
            if (string.IsNullOrWhiteSpace(logContentsStr))
            {
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(logContentsStr)}')");
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
                        _logger.LogError($"Sensor type {line[0]} has not been implemented yet.");
                        throw new NotImplementedException($"Sensor type {line[0]} has not been implemented yet.");
                }
            }

            return model;
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
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(timestampInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(timestampInputValue));
            }

            if (string.IsNullOrWhiteSpace(decimalInputValue))
            {
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(decimalInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(decimalInputValue));
            }

            if (!DateTime.TryParse(timestampInputValue, out DateTime timestamp))
            {
                _logger.LogError($"{Messages.InvalidDateTimeValue} (Parameter '{nameof(timestampInputValue)}')");
                throw new ArgumentException(Messages.InvalidDateTimeValue, nameof(timestampInputValue));
            }

            if (!decimal.TryParse(decimalInputValue, out decimal value))
            {
                _logger.LogError($"{Messages.InvalidDecimalValue} (Parameter '{nameof(decimalInputValue)}')");
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
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(timestampInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(timestampInputValue));
            }

            if (string.IsNullOrWhiteSpace(integerInputValue))
            {
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(integerInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(integerInputValue));
            }

            if (!DateTime.TryParse(timestampInputValue, out DateTime timestamp))
            {
                _logger.LogError($"{Messages.InvalidDateTimeValue} (Parameter '{nameof(timestampInputValue)}')");
                throw new ArgumentException(Messages.InvalidDateTimeValue, nameof(timestampInputValue));
            }

            if (!int.TryParse(integerInputValue, out int value))
            {
                _logger.LogError($"{Messages.InvalidIntegerValue} (Parameter '{nameof(integerInputValue)}')");
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
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(thermometerInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(thermometerInputValue));
            }

            if (string.IsNullOrWhiteSpace(humidityInputValue))
            {
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(humidityInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(humidityInputValue));
            }

            if (string.IsNullOrWhiteSpace(monoxideInputValue))
            {
                _logger.LogError($"{Messages.NullWhitespaceString} (Parameter '{nameof(monoxideInputValue)}')");
                throw new ArgumentException(Messages.NullWhitespaceString, nameof(monoxideInputValue));
            }

            if (!decimal.TryParse(thermometerInputValue, out decimal thermometerValue))
            {
                _logger.LogError($"{Messages.InvalidDecimalValue} (Parameter '{nameof(thermometerInputValue)}')");
                throw new ArgumentException(Messages.InvalidDecimalValue, nameof(thermometerInputValue));
            }

            if (!decimal.TryParse(humidityInputValue, out decimal humidityValue))
            {
                _logger.LogError($"{Messages.InvalidDecimalValue} (Parameter '{nameof(humidityInputValue)}')");
                throw new ArgumentException(Messages.InvalidDecimalValue, nameof(humidityInputValue));
            }

            if (!int.TryParse(monoxideInputValue, out int monoxideValue))
            {
                _logger.LogError($"{Messages.InvalidIntegerValue} (Parameter '{nameof(monoxideInputValue)}')");
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
