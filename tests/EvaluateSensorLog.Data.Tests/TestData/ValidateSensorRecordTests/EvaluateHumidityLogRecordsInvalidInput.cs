using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
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
