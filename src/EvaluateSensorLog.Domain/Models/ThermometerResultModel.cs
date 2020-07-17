namespace EvaluateSensorLog.Domain.Models
{
    public class ThermometerResultModel
    {
        public string SensorName { get; set; }

        public ThermometerStatus ThermometerStatus { get; set; }
    }
}