﻿using System;
using System.Linq;
using System.Reflection;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using EvaluateSensorLog.ClassLibrary.Models;
using EvaluateSensorLog.ClassLibrary.Tests.TestData.ParseSensorRecordFileTests;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.ClassLibrary.Tests
{
    public class ParseSensorRecordFileTests
    {
        private readonly object _logFile;
        private readonly Mock<ILogger<ParseSensorRecordFile>> _logger;
        private readonly MethodInfo[] _methodInfos;
        private readonly IParseSensorRecordFile _parseSensorRecordFile;
        private readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordFileTests`1"/> class.
        /// </summary>
        public ParseSensorRecordFileTests()
        {
            _logger = new Mock<ILogger<ParseSensorRecordFile>>();
            _parseSensorRecordFile = new ParseSensorRecordFile(_logger.Object);
            _type = typeof(ParseSensorRecordFile);
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
                .BeEquivalentTo(expected);
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
                .BeEquivalentTo(expected);
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
                .BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(ParseInputLogFileInvalidInput))]
        public void ParseInputLogFileInvalidInputTest(string logContentsStr, string expected)
        {
            // Arrange

            // Act
            Action actual = () => _parseSensorRecordFile.ParseInputLogFile(logContentsStr);

            // Assert
            actual
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(expected);
        }

        [Theory]
        [ClassData(typeof(ParseInputLogFile))]
        public void ParseInputLogFileTest(string logContentsStr, SensorLogModel expected)
        {
            // Arrange

            // Act
            SensorLogModel actual = _parseSensorRecordFile.ParseInputLogFile(logContentsStr);

            // Assert
            actual
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}