using System;
using System.Collections.Generic;
using System.Text.Json;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using EvaluateSensorLog.ClassLibrary.Models;
using EvaluateSensorLog.ClassLibrary.Tests.TestData.EvaluateSensorLogRecordsTests;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.ClassLibrary.Tests
{
    public class EvaluateSensorLogRecordsTests
    {
        private readonly IEvaluateSensorLogRecords _evaluateSensorLogRecords;
        private readonly Mock<ILogger<EvaluateSensorLogRecords>> _logger;
        private readonly Mock<IParseSensorRecordFile> _parseSensorRecordFile;
        private readonly Mock<IValidateSensorRecord> _validateSensorRecord;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateSensorLogRecordsTests`1"/> class.
        /// </summary>
        public EvaluateSensorLogRecordsTests()
        {
            _logger = new Mock<ILogger<EvaluateSensorLogRecords>>();
            _parseSensorRecordFile = new Mock<IParseSensorRecordFile>();
            _validateSensorRecord = new Mock<IValidateSensorRecord>();
            _evaluateSensorLogRecords = new EvaluateSensorLogRecords(_logger.Object, _parseSensorRecordFile.Object, _validateSensorRecord.Object);
        }

        [Theory]
        [ClassData(typeof(EvaluateLogFileInvalidInput))]
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
        [ClassData(typeof(EvaluateLogFile))]
        public void EvaluateLogFileTest(string logContentsStr, string expected)
        {
            // Arrange
            SensorLogModel sensorLogModel = new SensorLogModel
            {
                ReferenceValues = new ReferenceValuesModel()
            };

            _parseSensorRecordFile
                .Setup(x => x.ParseInputLogFile(It.IsAny<string>()))
                .Returns(sensorLogModel);

            _validateSensorRecord
                .Setup(x => x.ValidateSensorLogRecords(It.IsAny<SensorLogModel>()))
                .Returns(expected);

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
    }
}