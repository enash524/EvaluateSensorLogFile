namespace EvaluateSensorLog.Domain.Models
{
    /// <summary>
    /// Sensor reference values
    /// </summary>
    public class ReferenceValuesModel
    {
        /// <summary>
        /// Thermometer sensor reference value
        /// </summary>
        public decimal ThermometerReferenceValue { get; set; }

        /// <summary>
        /// Humidity sensor reference value
        /// </summary>
        public decimal HumidityReferenceValue { get; set; }

        /// <summary>
        /// Monoxide sensor reference value
        /// </summary>
        public int MonoxideReferenceValue { get; set; }
    }
}