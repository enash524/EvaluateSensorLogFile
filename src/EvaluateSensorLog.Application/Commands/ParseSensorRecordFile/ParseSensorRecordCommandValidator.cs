using System.IO.Abstractions;
using FluentValidation;

namespace EvaluateSensorLog.Application.Commands.ParseSensorRecordFile
{
    /// <summary>
    /// Validates the ParseSensorRecordCommand parameters
    /// </summary>
    public class ParseSensorRecordCommandValidator : AbstractValidator<ParseSensorRecordCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSensorRecordCommandValidator`1"/> class.
        /// </summary>
        public ParseSensorRecordCommandValidator(IFileSystem fileSystem)
        {
            RuleFor(x => !string.IsNullOrWhiteSpace(x.Path));
            RuleFor(x => fileSystem.File.Exists(x.Path));
        }
    }
}