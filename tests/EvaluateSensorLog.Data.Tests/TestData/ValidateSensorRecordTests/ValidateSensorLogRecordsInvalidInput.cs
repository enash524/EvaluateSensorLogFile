using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class ValidateSensorLogRecordsInvalidInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullValue} (Parameter 'sensorLogModel')" };
            yield return new object[] { new SensorLogModel(), $"{Messages.NullValue} (Parameter 'ReferenceValues')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
