using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Interfaces
{
    /// <summary>
    /// Defines the methods for validating a sensor record
    /// </summary>
    public interface IValidateSensorRecordRepository
    {
        /// <summary>
        /// Reads the sensors and the log records and generates the quality control evaluation for the sensors
        /// </summary>
        /// <param name="sensorLogModel">The sensor log input model</param>
        /// <exception cref="ArgumentException">sensorLogModel is null or sensorLogModel.ReverenceValues is null</exception>
        /// <returns>A model representing the sensor log quality control evaluation</returns>
        ValidateSensorLogModel ValidateSensorLogRecords(SensorLogModel sensorLogModel);
    }
}
