using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.Domain.Models;
using MediatR;

namespace EvaluateSensorLog.Application.Commands.ValidateSensorRecord
{
    /// <summary>
    ///Provides the properties needed to run the ValidateSensorRecordCommandHandler
    /// </summary>
    public class ValidateSensorRecordCommand : IRequest<CommandResult<string>>
    {
        /// <summary>
        /// The SensorLogModel to validate
        /// </summary>
        public SensorLogModel SensorLogModel { get; set; }
    }
}