using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class EvaluateMonoxideLogRecordsTestInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, 0, $"{Messages.NullEmptyCollection} (Parameter 'monoxideModel')" };
            yield return new object[] { new List<MonoxideModel>(), 0, $"{Messages.NullEmptyCollection} (Parameter 'monoxideModel')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}