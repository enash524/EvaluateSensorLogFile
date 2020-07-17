using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Commands.ParseSensorRecordFile;
using EvaluateSensorLog.Application.Commands.ValidateSensorRecord;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.ClassLibrary.Interfaces;
using EvaluateSensorLog.ClassLibrary.Tests.TestData.EvaluateSensorLogRecordsTests;
using EvaluateSensorLog.Domain.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.ClassLibrary.Tests
{
    public class EvaluateSensorLogRecordsTests
    {
        private readonly IEvaluateSensorLogRecords _evaluateSensorLogRecords;
        private readonly Mock<ILogger<EvaluateSensorLogRecords>> _logger;
        private readonly Mock<IMediator> _mediatr;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateSensorLogRecordsTests`1"/> class.
        /// </summary>
        public EvaluateSensorLogRecordsTests()
        {
            _logger = new Mock<ILogger<EvaluateSensorLogRecords>>();
            _mediatr = new Mock<IMediator>();
            _evaluateSensorLogRecords = new EvaluateSensorLogRecords(_logger.Object, _mediatr.Object);
        }

        [Theory]
        [ClassData(typeof(EvaluateLogFile))]
        public async Task EvaluateLogFileTest(string path, ValidateSensorLogModel validateSensorLogModel, string expected)
        {
            // Arrange
            _mediatr
                .Setup(x => x.Send(It.IsAny<ParseSensorRecordCommand>(), default))
                .ReturnsAsync(new CommandResult<SensorLogModel>());

            _mediatr
                .Setup(x => x.Send(It.IsAny<ValidateSensorRecordCommand>(), default))
                .ReturnsAsync(new CommandResult<ValidateSensorLogModel> { Result = validateSensorLogModel });

            // Act
            string actual = await _evaluateSensorLogRecords.EvaluateLogFileAsync(path);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .NotBeNull()
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

        [Fact]
        public async Task Invalid_Sensor_Log_Model_Returns_Null()
        {
            // Arrange
            string path = @"C:\temp\file.txt";
            string expected = $"Send Sensor Log Command Async with Path: {path} produced errors and returned status: {CommandResultType.Error}";

            _mediatr
                .Setup(x => x.Send(It.IsAny<ParseSensorRecordCommand>(), default))
                .ReturnsAsync(new CommandResult<SensorLogModel> { CommandResultType = CommandResultType.Error });

            _mediatr
                .Setup(x => x.Send(It.IsAny<ValidateSensorRecordCommand>(), default));

            // Act
            string actual = await _evaluateSensorLogRecords.EvaluateLogFileAsync(path);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .BeNull();

                _mediatr
                    .Verify(x => x.Send(It.IsAny<ParseSensorRecordCommand>(), default), Times.Once());

                _mediatr
                    .Verify(x => x.Send(It.IsAny<ValidateSensorRecordCommand>(), default), Times.Never());

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

        [Fact]
        public async Task Invalid_Validate_Sensor_Log_Model_Returns_Null()
        {
            // Arrange
            string path = @"C:\temp\file.txt";
            string expected = $"Send Validate Sensor Record Command Async with Path: {path} produced errors and returned status: {CommandResultType.Error}";

            _mediatr
                .Setup(x => x.Send(It.IsAny<ParseSensorRecordCommand>(), default))
                .ReturnsAsync(new CommandResult<SensorLogModel> { CommandResultType = CommandResultType.Success });

            _mediatr
                .Setup(x => x.Send(It.IsAny<ValidateSensorRecordCommand>(), default))
                .ReturnsAsync(new CommandResult<ValidateSensorLogModel>() { CommandResultType = CommandResultType.Error });

            // Act
            string actual = await _evaluateSensorLogRecords.EvaluateLogFileAsync(path);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .BeNull();

                _mediatr
                    .Verify(x => x.Send(It.IsAny<ParseSensorRecordCommand>(), default), Times.Once());

                _mediatr
                    .Verify(x => x.Send(It.IsAny<ValidateSensorRecordCommand>(), default), Times.Once());

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
    }
}