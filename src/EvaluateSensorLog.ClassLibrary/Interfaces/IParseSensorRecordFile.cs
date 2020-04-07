using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Interfaces
{
    public interface IParseSensorRecordFile
    {
        SensorLogModel ParseInputLogFile(string logContentsStr);
    }
}