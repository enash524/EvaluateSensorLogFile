using System.Collections.Generic;

namespace EvaluateSensorLogFile.ClassLibrary.Models
{
    /// <summary>
    /// Represents a humidity sensor name and sensor reading records
    /// </summary>
    public class HumidityModel
    {
        /// <summary>
        /// Humidity sensor name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of humidity sensor reading records
        /// </summary>
        public List<ReadingModel<decimal>> Readings { get; set; } = new List<ReadingModel<decimal>>();
    }
}