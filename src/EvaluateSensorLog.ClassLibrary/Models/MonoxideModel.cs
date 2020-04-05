using System.Collections.Generic;

namespace EvaluateSensorLog.ClassLibrary.Models
{
    /// <summary>
    /// Represents a monoxide sensor name and sensor reading records
    /// </summary>
    public class MonoxideModel
    {
        /// <summary>
        /// Monoxide sensor name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of monoxide sensor reading records
        /// </summary>
        public List<IntegerReadingModel> Readings { get; set; } = new List<IntegerReadingModel>();
    }
}