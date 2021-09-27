using System.Collections.Generic;

namespace EvaluateSensorLog.Domain.Models
{
    public class ValidateSensorLogModel
    {
        public List<HumidityResultModel> HumidityResults { get; set; } = new List<HumidityResultModel>();

        public List<MonoxideResultModel> MonoxideResults { get; set; } = new List<MonoxideResultModel>();

        public List<ThermometerResultModel> ThermometerResults { get; set; } = new List<ThermometerResultModel>();
    }
}
