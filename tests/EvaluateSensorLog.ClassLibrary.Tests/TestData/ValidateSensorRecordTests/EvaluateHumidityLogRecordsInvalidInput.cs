using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData.ValidateSensorRecordTestsData
{
    public class EvaluateHumidityLogRecordsInvalidInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, 0, $"{Messages.NullEmptyCollection} (Parameter 'humidityModel')" };
            yield return new object[] { new List<HumidityModel>(), 0, $"{Messages.NullEmptyCollection} (Parameter 'humidityModel')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}