using System.Collections.Generic;

namespace EvaluateSensorLogFile.ClassLibrary.Models
{
    /// <summary>
    /// Represents a thermometer sensor name and sensor reading records
    /// </summary>
    public class ThermometerModel
    {
        /// <summary>
        /// Thermometer sensor name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of thermometer sensor reading records
        /// </summary>
        public List<ReadingModel<decimal>> Readings { get; set; } = new List<ReadingModel<decimal>>();
    }
}