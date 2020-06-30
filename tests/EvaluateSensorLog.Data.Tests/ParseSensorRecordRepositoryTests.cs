using System;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Data.Repositories;
using EvaluateSensorLog.Data.Tests.TestData.ParseSensorRecordFileTests;
using EvaluateSensorLog.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.Data.Tests
{
    public class ParseSensorRecordRepositoryTests
    {
        private readonly object _logFile;
        private readonly Mock<IFileSystem> _fileSystem;
        private readonly Mock<ILogger<ParseSensorRecordRepository>> _logger;
        private readonly MethodInfo[] _methodInfos;
        private readonly IParseSensorRecordRepository _parseSensorRecordRepository;
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordFileTests`1"/> class.
        /// </summary>
        public ParseSensorRecordRepositoryTests()
        {
            _fileSystem = new Mock<IFileSystem>();
            _logger = new Mock<ILogger<ParseSensorRecordRepository>>();
            _parseSensorRecordRepository = new ParseSensorRecordRepository(_fileSystem.Object, _logger.Object);
            _type = typeof(ParseSensorRecordRepository);
            _logFile = Activator.CreateInstance(_type, _logger.Object);
            _methodInfos = _type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [Theory]
        [ClassData(typeof(HandleDecimalReadingInvalidInput))]
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

        [Theory]
        [ClassData(typeof(HandleDecimalReading))]
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
                .NotBeNull()
                .And
                .Equals(expected);
        }

        [Theory]
        [ClassData(typeof(HandleIntegerReadingInvalidInput))]
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

        [Theory]
        [ClassData(typeof(HandleIntegerReading))]
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
                .NotBeNull()
                .And
                .Equals(expected);
        }

        [Theory]
        [ClassData(typeof(HandleReferenceValuesLogRecordInvalidInput))]
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

        [Theory]
        [ClassData(typeof(HandleReferenceValuesLogRecord))]
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
                .NotBeNull()
                .And
                .Equals(expected);
        }

        [Theory]
        [ClassData(typeof(ParseInputLogFileInvalidInput))]
        public void ParseInputLogFileInvalidInputTest(string path, string expected)
        {
            // Arrange

            // Act
            // TODO - NEED TO FIX THIS TEST!!!
            Action actual = () => _parseSensorRecordRepository.ParseInputLogFileAsync(path);

            // Assert
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

        [Theory]
        [ClassData(typeof(ParseInputLogFile))]
        public async Task ParseInputLogFileTest(string path, SensorLogModel expected)
        {
            // Arrange

            // Act
            // TODO - NEED TO FIX THIS TEST!!!
            SensorLogModel actual = await _parseSensorRecordRepository.ParseInputLogFileAsync(path);

            // Assert
            actual
                .Should()
                .NotBeNull()
                .And
                .Equals(expected);
        }
    }
}