using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData
{
    public class EvaluateThermometerLogRecordsInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, 0, $"{Messages.NullEmptyCollection} (Parameter 'thermometerModel')" };
            yield return new object[] { new List<ThermometerModel>(), 0, $"{Messages.NullEmptyCollection} (Parameter 'thermometerModel')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}