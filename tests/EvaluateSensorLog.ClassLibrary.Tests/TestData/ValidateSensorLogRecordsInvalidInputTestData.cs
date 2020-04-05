using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData
{
    public class ValidateSensorLogRecordsInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullValue} (Parameter 'sensorLogModel')" };
            yield return new object[] { new SensorLogModel(), $"{Messages.NullValue} (Parameter 'ReferenceValues')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}