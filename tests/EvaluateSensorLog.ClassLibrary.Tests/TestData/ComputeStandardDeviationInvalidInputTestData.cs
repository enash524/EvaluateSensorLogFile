using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData
{
    public class ComputeStandardDeviationInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullEmptyCollection} (Parameter 'readings')" };
            yield return new object[] { new List<DecimalReadingModel>(), $"{Messages.NullEmptyCollection} (Parameter 'readings')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}