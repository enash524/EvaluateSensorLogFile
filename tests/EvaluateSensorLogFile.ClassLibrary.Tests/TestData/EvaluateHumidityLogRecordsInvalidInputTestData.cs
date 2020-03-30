using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class EvaluateHumidityLogRecordsInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, 0, $"{Messages.NullEmptyCollection} (Parameter 'humidityModel')" };
            yield return new object[] { new List<HumidityModel>(), 0, $"{Messages.NullEmptyCollection} (Parameter 'humidityModel')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}