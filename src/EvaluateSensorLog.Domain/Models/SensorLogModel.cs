using System.Collections.Generic;

namespace EvaluateSensorLog.Domain.Models
{
    /// <summary>
    /// Represents the collection of reference values and sensor reading records
    /// </summary>
    public class SensorLogModel
    {
        /// <summary>
        /// Sensor reference values
        /// </summary>
        public ReferenceValuesModel ReferenceValues { get; set; }

        /// <summary>
        /// Collection of thermometer sensor reading records
        /// </summary>
        public List<ThermometerModel> ThermometerReadings { get; set; } = new List<ThermometerModel>();

        /// <summary>
        /// Collection of humidity sensor reading records
        /// </summary>
        public List<HumidityModel> HumidityReadings { get; set; } = new List<HumidityModel>();

        /// <summary>
        /// Collection of monoxide sensor reading records
        /// </summary>
        public List<MonoxideModel> MonoxideReadings { get; set; } = new List<MonoxideModel>();
    }
}