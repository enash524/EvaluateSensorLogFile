using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Commands.ParseSensorRecordFile;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Domain.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.Application.Tests.Commands.ParseSensorRecordFile
{
    public class ParseSensorRecordCommandHandlerTests
    {
        private readonly Mock<IFileSystem> _fileSystem;
        private readonly ParseSensorRecordCommandHandler _handler;
        private readonly Mock<ILogger<ParseSensorRecordCommandHandler>> _logger;
        private readonly Mock<IParseSensorRecordRepository> _repository;
        private readonly IValidator<ParseSensorRecordCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordCommandHandlerTests`1"/> class.
        /// </summary>
        public ParseSensorRecordCommandHandlerTests()
        {
            _fileSystem = new Mock<IFileSystem>();
            _logger = new Mock<ILogger<ParseSensorRecordCommandHandler>>();
            _repository = new Mock<IParseSensorRecordRepository>();
            _validator = new ParseSensorRecordCommandValidator(_fileSystem.Object);
            _handler = new ParseSensorRecordCommandHandler(_logger.Object, _repository.Object, _validator);
        }

        [Fact]
        public async Task Handle_FileDoesNotExist_Test()
        {
            // Arrange
            string path = @"C:\temp\file.txt";
            string expectedMessage = $"Parse Sensor Record Command with Path: {path} produced errors on validation: {path} does not exist.";
            CommandResult<SensorLogModel> expectedResult = new CommandResult<SensorLogModel>
            {
                Result = null,
                CommandResultType = CommandResultType.InvalidInput
            };
            ParseSensorRecordCommand command = new ParseSensorRecordCommand
            {
                Path = path
            };

            _fileSystem
                .Setup(f => f.File.Exists(It.IsAny<string>()))
                .Returns(false);

            // Act
            CommandResult<SensorLogModel> actual = await _handler.Handle(command);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .NotBeNull()
                    .And
                    .BeEquivalentTo(expectedResult);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expectedMessage)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Fact]
        public async Task Handle_Null_SensorLogModel_Result_Test()
        {
            // Arrange
            string path = @"C:\temp\file.txt";
            string expectedMessage = $"Parse Sensor Record Command with Path: {path} produced an invalid result.";
            CommandResult<SensorLogModel> expectedResult = new CommandResult<SensorLogModel>
            {
                Result = null,
                CommandResultType = CommandResultType.Error
            };
            ParseSensorRecordCommand command = new ParseSensorRecordCommand
            {
                Path = path
            };

            _fileSystem
                .Setup(f => f.File.Exists(It.IsAny<string>()))
                .Returns(true);

            _repository
                .Setup(r => r.ParseInputLogFileAsync(It.IsAny<string>()))
                .ReturnsAsync((SensorLogModel)null);

            // Act
            CommandResult<SensorLogModel> actual = await _handler.Handle(command);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .NotBeNull()
                    .And
                    .BeEquivalentTo(expectedResult);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expectedMessage)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Fact]
        public async Task Handle_NullOrWhitespace_Path_Test()
        {
            // Arrange
            string expectedMessage = "Parse Sensor Record Command with Path: (null) produced errors on validation: Path cannot be null or whitespace.";
            CommandResult<SensorLogModel> expectedResult = new CommandResult<SensorLogModel>
            {
                Result = null,
                CommandResultType = CommandResultType.InvalidInput
            };
            ParseSensorRecordCommand command = new ParseSensorRecordCommand
            {
                Path = null
            };

            // Act
            CommandResult<SensorLogModel> actual = await _handler.Handle(command);

            // Assert
            using (new AssertionScope())
            {
                actual
                    .Should()
                    .NotBeNull()
                    .And
                    .BeEquivalentTo(expectedResult);

                _logger.Invocations.Count
                    .Should()
                    .Be(1);

                _logger.Invocations[0].Arguments[0]
                    .Should()
                    .Be(LogLevel.Error);

                _logger
                    .Verify(x => x.Log(LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((x, t) => string.Equals(x.ToString(), expectedMessage)),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                        Times.Once());
            }
        }

        [Fact]
        public async Task Hanlde_Should_Return_Valid_Result_Test()
        {
            // Arrange
            string path = @"C:\temp\file.txt";
            SensorLogModel expectedModel = new SensorLogModel();
            CommandResult<SensorLogModel> expectedResult = new CommandResult<SensorLogModel>
            {
                Result = expectedModel,
                CommandResultType = CommandResultType.Success
            };
            ParseSensorRecordCommand command = new ParseSensorRecordCommand
            {
                Path = path
            };

            _fileSystem
                .Setup(f => f.File.Exists(It.IsAny<string>()))
                .Returns(true);

            _repository
                .Setup(r => r.ParseInputLogFileAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedModel);

            // Act
            CommandResult<SensorLogModel> actual = await _handler.Handle(command);

            // Assert
            actual
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(expectedResult);
        }
    }
}