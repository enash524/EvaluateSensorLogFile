using System.Threading.Tasks;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Interfaces
{
    /// <summary>
    /// Defines the methods for parsing a sensor log file
    /// </summary>
    public interface IParseSensorRecordRepository
    {
        /// <summary>
        /// Parses an input file and returns the resulting sensor log model
        /// </summary>
        /// <param name="path">The path to the sensor log record to parse</param>
        /// <returns>A task representing the resulting sensor log model</returns>
        Task<SensorLogModel> ParseInputLogFileAsync(string path);
    }
}