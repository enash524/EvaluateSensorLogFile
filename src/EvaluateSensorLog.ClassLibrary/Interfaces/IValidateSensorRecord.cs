using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Interfaces
{
    public interface IValidateSensorRecord
    {
        string ValidateSensorLogRecords(SensorLogModel sensorLogModel);
    }
}