using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class EvaluateSensorLogRecordsInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullValue} (Parameter 'sensorLogModel')" };
            yield return new object[] { new SensorLogModel(), $"{Messages.NullValue} (Parameter 'ReferenceValues')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}