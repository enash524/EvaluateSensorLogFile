using FluentValidation;

namespace EvaluateSensorLog.Application.Commands.ValidateSensorRecord
{
    /// <summary>
    /// Validates the ValidateSensorRecordCommand parameters
    /// </summary>
    public class ValidateSensorRecordCommandValidator : AbstractValidator<ValidateSensorRecordCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSensorRecordCommandValidator`1"/> class.
        /// </summary>
        public ValidateSensorRecordCommandValidator()
        {
            RuleFor(x => x.SensorLogModel).NotNull();
        }
    }
}