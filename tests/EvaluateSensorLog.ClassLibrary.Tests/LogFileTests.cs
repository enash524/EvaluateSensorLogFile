using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using EvaluateSensorLog.ClassLibrary.Models;
using EvaluateSensorLog.ClassLibrary.Tests.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.ClassLibrary.Tests
{
    public class LogFileTests
    {
        private readonly IEvaluateSensorLogRecords _evaluateSensorLogRecords;
        private readonly Mock<ILogger<EvaluateSensorLogRecords>> _logger;
        private readonly Type _type;
        private readonly object _logFile;
        private readonly MethodInfo[] _methodInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileTests`1"/> class.
        /// </summary>
        public LogFileTests()
        {
            _logger = new Mock<ILogger<EvaluateSensorLogRecords>>();
            _evaluateSensorLogRecords = new EvaluateSensorLogRecords(_logger.Object);
            _type = typeof(EvaluateSensorLogRecords);
            _logFile = Activator.CreateInstance(_type, _logger.Object);
            _methodInfos = _type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [Theory]
        [ClassData(typeof(ComputeStandardDeviationInvalidInputTestData))]
        public void ComputeStandardDeviationInvalidInputTestData(List<DecimalReadingModel> readings, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("ComputeStandardDeviation") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { readings });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(ComputeStandardDeviationTestData))]
        public void ComputeStandardDeviationTest(List<DecimalReadingModel> readings, decimal expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("ComputeStandardDeviation") && x.IsPrivate)
                .First();

            // Act
            decimal actual = (decimal)methodInfo.Invoke(_logFile, new object[] { readings });
            actual = Math.Round(actual, 1);

            // Assert
            actual
                .Should()
                .Be(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateHumidityLogRecordsInvalidInputTestData))]
        public void EvaluateHumidityLogRecordsInvalidInputTest(List<HumidityModel> humidityModel, decimal referenceValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateHumidityLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { humidityModel, referenceValue });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateHumidityLogRecordsTestData))]
        public void EvaluateHumidityLogRecordsTest(List<HumidityModel> humidityModel, decimal referenceValue, List<KeyValuePair<string, string>> expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateHumidityLogRecords") && x.IsPrivate)
                .First();

            // Act
            List<KeyValuePair<string, string>> actual = (List<KeyValuePair<string, string>>)methodInfo.Invoke(_logFile, new object[] { humidityModel, referenceValue });

            // Assert
            actual
                .Should()
                .NotBeNull()
                .And
                .HaveCount(expected.Count)
                .And
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateInputLogFileInvalidInputTestData))]
        public void EvaluateInputLogFileInvalidInputTest(string logContentsStr, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateInputLogFile") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { logContentsStr });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateInputLogFileTestData))]
        public void EvaluateInputLogFileTest(string logContentsStr, SensorLogModel expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateInputLogFile") && x.IsPrivate)
                .First();

            // Act
            SensorLogModel actual = (SensorLogModel)methodInfo.Invoke(_logFile, new object[] { logContentsStr });

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateLogFileInvalidInputTestData))]
        public void EvaluateLogFileInvalidInputTest(string logContentsStr, string expected)
        {
            // Arrange

            // Act
            Action actual = () => _evaluateSensorLogRecords.EvaluateLogFile(logContentsStr);

            // Assert
            actual
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateLogFileTestData))]
        public void EvaluateLogFileTest(string logContentsStr, string expected)
        {
            // Arrange

            // Act
            string actual = _evaluateSensorLogRecords.EvaluateLogFile(logContentsStr);

            // Assert
            actual
                .Should()
                .NotBeNullOrWhiteSpace()
                .And
                .BeEquivalentTo(expected);

            IDictionary<string, string> actualDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(actual);

            actualDictionary
                .Should()
                .Contain("temp-1", "precise")
                .And
                .Contain("temp-2", "ultra precise")
                .And
                .Contain("hum-1", "keep")
                .And
                .Contain("hum-2", "discard")
                .And
                .Contain("mon-1", "keep")
                .And
                .Contain("mon-2", "discard");
        }

        [Theory]
        [ClassData(typeof(EvaluateMonoxideLogRecordsTestData))]
        public void EvaluateMonoxideLogRecordsTest(List<MonoxideModel> monoxideModel, int referenceValue, List<KeyValuePair<string, string>> expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateMonoxideLogRecords") && x.IsPrivate)
                .First();

            // Act
            List<KeyValuePair<string, string>> actual = (List<KeyValuePair<string, string>>)methodInfo.Invoke(_logFile, new object[] { monoxideModel, referenceValue });

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateMonoxideLogRecordsTestInvalidInputTestData))]
        public void EvaluateMonoxideLogRecordsTestInvalidInputTest(List<MonoxideModel> monoxideModel, int referenceValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateMonoxideLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { monoxideModel, referenceValue });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateThermometerLogRecordsInvalidInputTestData))]
        public void EvaluateThermometerLogRecordsInvalidInputTest(List<ThermometerModel> thermometerModel, decimal referenceValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateThermometerLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { thermometerModel, referenceValue });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(EvaluateThermometerLogRecordsTestData))]
        public void EvaluateThermometerLogRecordsTest(List<ThermometerModel> thermometerModel, decimal referenceValue, List<KeyValuePair<string, string>> expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateThermometerLogRecords") && x.IsPrivate)
                .First();

            // Act
            List<KeyValuePair<string, string>> actual = (List<KeyValuePair<string, string>>)methodInfo.Invoke(_logFile, new object[] { thermometerModel, referenceValue });

            // Assert
            actual
                .Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(expected.Count)
                .And
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(HandleDecimalReadingInvalidInputTestData))]
        public void HandleDecimalReadingInvalidInputTest(string timestampInputValue, string decimalInputValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("HandleDecimalReading") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { timestampInputValue, decimalInputValue });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(HandleDecimalReadingTestData))]
        public void HandleDecimalReadingTest(string timestampInputValue, string decimalInputValue, DecimalReadingModel expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("HandleDecimalReading") && x.IsPrivate)
                .First();

            // Act
            DecimalReadingModel actual = (DecimalReadingModel)methodInfo.Invoke(_logFile, new object[] { timestampInputValue, decimalInputValue });

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(HandleIntegerReadingInvalidInputTestData))]
        public void HandleIntegerReadingInvalidInputTest(string timestampInputValue, string integerInputValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("HandleIntegerReading") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { timestampInputValue, integerInputValue });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(HandleIntegerReadingTestData))]
        public void HandleIntegerReadingTest(string timestampInputValue, string integerInputValue, IntegerReadingModel expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("HandleIntegerReading") && x.IsPrivate)
                .First();

            // Act
            IntegerReadingModel actual = (IntegerReadingModel)methodInfo.Invoke(_logFile, new object[] { timestampInputValue, integerInputValue });

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(HandleReferenceValuesLogRecordInvalidInputTestData))]
        public void HandleReferenceValuesLogRecordInvalidInputTest(string thermometerInputValue, string humidityInputValue, string monoxideInputValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("HandleReferenceValuesLogRecord") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { thermometerInputValue, humidityInputValue, monoxideInputValue });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(HandleReferenceValuesLogRecordTestData))]
        public void HandleReferenceValuesLogRecordTest(string thermometerInputValue, string humidityInputValue, string monoxideInputValue, ReferenceValuesModel expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("HandleReferenceValuesLogRecord") && x.IsPrivate)
                .First();

            // Act
            ReferenceValuesModel actual = (ReferenceValuesModel)methodInfo.Invoke(_logFile, new object[] { thermometerInputValue, humidityInputValue, monoxideInputValue });

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(ValidateSensorLogRecordsInvalidInputTestData))]
        public void ValidateSensorLogRecordsInvalidInputTest(SensorLogModel sensorLogModel, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("ValidateSensorLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { sensorLogModel });

            // Assert
            actual
                .Should()
                .Throw<TargetInvocationException>()
                .WithInnerException<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(ValidateSensorLogRecordsTestData))]
        public void ValidateSensorLogRecordsTest(SensorLogModel sensorLogModel, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("ValidateSensorLogRecords") && x.IsPrivate)
                .First();

            // Act
            string actual = (string)methodInfo.Invoke(_logFile, new object[] { sensorLogModel });

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}