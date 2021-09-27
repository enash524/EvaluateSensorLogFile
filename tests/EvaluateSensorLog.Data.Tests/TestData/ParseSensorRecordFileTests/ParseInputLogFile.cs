using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ParseSensorRecordFileTests
{
    public class ParseInputLogFile : IEnumerable<object[]>
    {
        private static readonly string[] input = new string[7]
        {
            "reference 70.0 45.0 6",
            "thermometer temp-1",
            "2007-04-05T22:00 72.4",
            "humidity hum-1",
            "2007-04-05T22:04 45.2",
            "monoxide mon-1",
            "2007-04-05T22:04 5"
        };

        private static readonly SensorLogModel sensorLogModel = new SensorLogModel();

        public ParseInputLogFile()
        {
            ReferenceValuesModel referenceValuesModel = new ReferenceValuesModel
            {
                ThermometerReferenceValue = 70.0m,
                HumidityReferenceValue = 45.0m,
                MonoxideReferenceValue = 6
            };

            ThermometerModel thermometerModel = new ThermometerModel
            {
                Name = "temp-1",
                Readings = new List<DecimalReadingModel> { new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 72.4m } }
            };

            HumidityModel humidityModel = new HumidityModel
            {
                Name = "hum-1",
                Readings = new List<DecimalReadingModel> { new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 45.2m } }
            };

            MonoxideModel monoxideModel = new MonoxideModel
            {
                Name = "mon-1",
                Readings = new List<IntegerReadingModel> { new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 5 } }
            };

            sensorLogModel.ReferenceValues = referenceValuesModel;
            sensorLogModel.ThermometerReadings.Add(thermometerModel);
            sensorLogModel.HumidityReadings.Add(humidityModel);
            sensorLogModel.MonoxideReadings.Add(monoxideModel);
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { string.Join(Environment.NewLine, input), sensorLogModel };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
