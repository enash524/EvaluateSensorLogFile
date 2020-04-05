using System.Collections.Generic;

namespace EvaluateSensorLog.ClassLibrary.Models
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
        public List<DecimalReadingModel> Readings { get; set; } = new List<DecimalReadingModel>();
    }
}