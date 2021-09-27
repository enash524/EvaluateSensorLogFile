using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Data.Repositories;
using EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests;
using EvaluateSensorLog.Domain.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.Data.Tests
{
    public class ValidateSensorRecordTests
    {
        private readonly object _logFile;
        private readonly Mock<ILogger<ValidateSensorRecordRepository>> _logger;
        private readonly MethodInfo[] _methodInfos;
        private readonly Type _type;
        private readonly IValidateSensorRecordRepository _validateSensorRecordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSensorRecordTests`1"/> class.
        /// </summary>
        public ValidateSensorRecordTests()
        {
            _logger = new Mock<ILogger<ValidateSensorRecordRepository>>();
            _validateSensorRecordRepository = new ValidateSensorRecordRepository(_logger.Object);
            _type = typeof(ValidateSensorRecordRepository);
            _logFile = Activator.CreateInstance(_type, _logger.Object);
            _methodInfos = _type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [Theory]
        [ClassData(typeof(ComputeStandardDeviationInvalidInput))]
        public void ComputeStandardDeviationInvalidInputTestData(List<DecimalReadingModel> readings, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("ComputeStandardDeviation") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { readings });

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .Throw<TargetInvocationException>()
                    .WithInnerException<ArgumentException>()
                    .WithMessage(expected);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expected)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Theory]
        [ClassData(typeof(ComputeStandardDeviation))]
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
        [ClassData(typeof(EvaluateHumidityLogRecordsInvalidInput))]
        public void EvaluateHumidityLogRecordsInvalidInputTest(List<HumidityModel> humidityModel, decimal referenceValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateHumidityLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { humidityModel, referenceValue });

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .Throw<TargetInvocationException>()
                    .WithInnerException<ArgumentException>()
                    .WithMessage(expected);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expected)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Theory]
        [ClassData(typeof(EvaluateHumidityLogRecords))]
        public void EvaluateHumidityLogRecordsTest(List<HumidityModel> humidityModel, decimal referenceValue, List<HumidityResultModel> expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateHumidityLogRecords") && x.IsPrivate)
                .First();

            // Act
            List<HumidityResultModel> actual = (List<HumidityResultModel>)methodInfo.Invoke(_logFile, new object[] { humidityModel, referenceValue });

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
        [ClassData(typeof(EvaluateMonoxideLogRecords))]
        public void EvaluateMonoxideLogRecordsTest(List<MonoxideModel> monoxideModel, int referenceValue, List<MonoxideResultModel> expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateMonoxideLogRecords") && x.IsPrivate)
                .First();

            // Act
            List<MonoxideResultModel> actual = (List<MonoxideResultModel>)methodInfo.Invoke(_logFile, new object[] { monoxideModel, referenceValue });

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
        [ClassData(typeof(EvaluateMonoxideLogRecordsTestInvalidInput))]
        public void EvaluateMonoxideLogRecordsTestInvalidInputTest(List<MonoxideModel> monoxideModel, int referenceValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateMonoxideLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { monoxideModel, referenceValue });

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .Throw<TargetInvocationException>()
                    .WithInnerException<ArgumentException>()
                    .WithMessage(expected);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expected)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Theory]
        [ClassData(typeof(EvaluateThermometerLogRecordsInvalidInput))]
        public void EvaluateThermometerLogRecordsInvalidInputTest(List<ThermometerModel> thermometerModel, decimal referenceValue, string expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateThermometerLogRecords") && x.IsPrivate)
                .First();

            // Act
            Action actual = () => methodInfo.Invoke(_logFile, new object[] { thermometerModel, referenceValue });

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .Throw<TargetInvocationException>()
                    .WithInnerException<ArgumentException>()
                    .WithMessage(expected);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expected)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Theory]
        [ClassData(typeof(EvaluateThermometerLogRecords))]
        public void EvaluateThermometerLogRecordsTest(List<ThermometerModel> thermometerModel, decimal referenceValue, List<ThermometerResultModel> expected)
        {
            // Arrange
            MethodInfo methodInfo = _methodInfos
                .Where(x => x.Name.Equals("EvaluateThermometerLogRecords") && x.IsPrivate)
                .First();

            // Act
            List<ThermometerResultModel> actual = (List<ThermometerResultModel>)methodInfo.Invoke(_logFile, new object[] { thermometerModel, referenceValue });

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
        [ClassData(typeof(ValidateSensorLogRecordsInvalidInput))]
        public void ValidateSensorLogRecordsInvalidInputTest(SensorLogModel sensorLogModel, string expected)
        {
            // Arrange

            // Act
            Action actual = () => _validateSensorRecordRepository.ValidateSensorLogRecords(sensorLogModel);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .Throw<ArgumentException>()
                    .WithMessage(expected);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expected)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Theory]
        [ClassData(typeof(ValidateSensorLogRecords))]
        public void ValidateSensorLogRecordsTest(SensorLogModel sensorLogModel, ValidateSensorLogModel expected)
        {
            // Arrange

            // Act
            ValidateSensorLogModel actual = _validateSensorRecordRepository.ValidateSensorLogRecords(sensorLogModel);

            // Assert
            actual
                .Should()
                .NotBeNull()
                .And
                .Equals(expected);
        }
    }
}
