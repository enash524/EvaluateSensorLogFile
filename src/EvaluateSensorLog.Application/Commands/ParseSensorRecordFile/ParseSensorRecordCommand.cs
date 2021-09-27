using EvaluateSensorLog.Application.Models;
using EvaluateSensorLog.Domain.Models;
using MediatR;

namespace EvaluateSensorLog.Application.Commands.ParseSensorRecordFile
{
    /// <summary>
    /// Provides the properties needed to run the ParseSensorRecordCommandHandler
    /// </summary>
    public class ParseSensorRecordCommand : IRequest<CommandResult<SensorLogModel>>
    {
        /// <summary>
        /// The path to the sensor record file to be parsed
        /// </summary>
        public string Path { get; set; }
    }
}
