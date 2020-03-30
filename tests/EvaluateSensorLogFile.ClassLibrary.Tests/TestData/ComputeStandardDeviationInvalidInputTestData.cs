using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class ComputeStandardDeviationInvalidInputTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullEmptyCollection} (Parameter 'readings')" };
            yield return new object[] { new List<ReadingModel<decimal>>(), $"{Messages.NullEmptyCollection} (Parameter 'readings')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}