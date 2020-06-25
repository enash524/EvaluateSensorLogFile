﻿using EvaluateSensorLog.ClassLibrary;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvaluateSensorLog.ClassLibrary.Tests
{
    public class EvaluateSensorLogRecordsTests
    {
        private readonly IEvaluateSensorLogRecords _evaluateSensorLogRecords;
        private readonly Mock<ILogger<EvaluateSensorLogRecords>> _logger;

        //private readonly Mock<IParseSensorRecordFile> _parseSensorRecordFile;
        //private readonly Mock<IValidateSensorRecord> _validateSensorRecord;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateSensorLogRecordsTests`1"/> class.
        /// </summary>
        public EvaluateSensorLogRecordsTests()
        {
            _logger = new Mock<ILogger<EvaluateSensorLogRecords>>();

            //_parseSensorRecordFile = new Mock<IParseSensorRecordFile>();
            //_validateSensorRecord = new Mock<IValidateSensorRecord>();
            //_evaluateSensorLogRecords = new EvaluateSensorLogRecords(_logger.Object, _parseSensorRecordFile.Object, _validateSensorRecord.Object);
        }

        /*
                [Theory]
                [ClassData(typeof(EvaluateLogFileInvalidInput))]
                public void EvaluateLogFileInvalidInputTest(string path, string expected)
                {
                    // Arrange

                    // Act
                    Action actual = () => _evaluateSensorLogRecords.EvaluateLogFileAsync(path);

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
        */
    }
}