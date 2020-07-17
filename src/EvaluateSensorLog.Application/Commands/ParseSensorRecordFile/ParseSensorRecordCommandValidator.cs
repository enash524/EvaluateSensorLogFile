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
            RuleFor(x => x.Path)
                .Custom((path, context) =>
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        context.AddFailure("Path cannot be null or whitespace.");
                    }
                    else
                    {
                        if (!fileSystem.File.Exists(path))
                        {
                            context.AddFailure($"{path} does not exist.");
                        }
                    }
                });
        }
    }
}