using System;
using System.Threading.Tasks;
using EvaluateSensorLog.Application.Commands.ValidateSensorRecord;
using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Domain.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvaluateSensorLog.Application.Tests.Commands.ValidateSensorRecord
{
    public class ValidateSensorRecordCommandHandlerTests
    {
        private readonly ValidateSensorRecordCommandHandler _handler;
        private readonly Mock<ILogger<ValidateSensorRecordCommandHandler>> _logger;
        private readonly Mock<IValidateSensorRecordRepository> _repository;
        private readonly IValidator<ValidateSensorRecordCommand> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSensorRecordCommandHandlerTests`1"/> class.
        /// </summary>
        public ValidateSensorRecordCommandHandlerTests()
        {
            _logger = new Mock<ILogger<ValidateSensorRecordCommandHandler>>();
            _repository = new Mock<IValidateSensorRecordRepository>();
            _validator = new ValidateSensorRecordCommandValidator();
            _handler = new ValidateSensorRecordCommandHandler(_logger.Object, _repository.Object, _validator);
        }

        [Fact]
        public async Task Handle_Null_Or_Empty_Result_Should_Produce_Validation_Error()
        {
            // Arrange
            string expectedMessage = "Validate Sensor Record produced an invalid result.";
            CommandResult<string> expectedResult = new CommandResult<string>
            {
                Result = null,
                CommandResultType = CommandResultType.Error
            };
            ValidateSensorRecordCommand command = new ValidateSensorRecordCommand
            {
                SensorLogModel = new SensorLogModel()
            };

            _repository
                .Setup(r => r.ValidateSensorLogRecords(It.IsAny<SensorLogModel>()))
                .Returns((ValidateSensorLogModel)null);

            // Act
            CommandResult<ValidateSensorLogModel> actual = await _handler.Handle(command);

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
        public async Task Handle_Null_SensorLogModel_Should_Produce_Validation_Error()
        {
            // Arrange
            string expectedMessage = "Validate Sensor Record produced errors: 'Sensor Log Model' must not be empty.";
            CommandResult<string> expectedResult = new CommandResult<string>
            {
                Result = null,
                CommandResultType = CommandResultType.InvalidInput
            };
            ValidateSensorRecordCommand command = new ValidateSensorRecordCommand
            {
                SensorLogModel = null
            };

            // Act
            CommandResult<ValidateSensorLogModel> actual = await _handler.Handle(command);

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
    }
}
