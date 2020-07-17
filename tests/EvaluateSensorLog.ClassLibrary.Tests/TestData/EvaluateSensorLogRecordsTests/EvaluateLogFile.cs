using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData.EvaluateSensorLogRecordsTests
{
    public class EvaluateLogFile : IEnumerable<object[]>
    {
        private const string _expected = "{\r\n  \"temp-1\": \"precise\",\r\n  \"temp-2\": \"ultra precise\",\r\n  \"hum-1\": \"keep\",\r\n  \"hum-2\": \"discard\",\r\n  \"mon-1\": \"keep\",\r\n  \"mon-2\": \"discard\"\r\n}";
        private const string _path = @"C:\temp\file.txt";

        private static readonly ValidateSensorLogModel _validateSensorLogModel = new ValidateSensorLogModel
        {
            HumidityResults = new List<HumidityResultModel>
            {
                new HumidityResultModel { SensorName = "hum-1", HumidityStatus = HumidityStatus.Keep },
                new HumidityResultModel { SensorName = "hum-2", HumidityStatus = HumidityStatus.Discard }
            },
            MonoxideResults = new List<MonoxideResultModel>
            {
                new MonoxideResultModel { SensorName = "mon-1", MonoxideStatus = MonoxideStatus.Keep },
                new MonoxideResultModel { SensorName = "mon-2", MonoxideStatus = MonoxideStatus.Discard }
            },
            ThermometerResults = new List<ThermometerResultModel>
            {
                new ThermometerResultModel { SensorName = "temp-1", ThermometerStatus = ThermometerStatus.Precise },
                new ThermometerResultModel { SensorName = "temp-2", ThermometerStatus = ThermometerStatus.UltraPrecise }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { _path, _validateSensorLogModel, _expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}