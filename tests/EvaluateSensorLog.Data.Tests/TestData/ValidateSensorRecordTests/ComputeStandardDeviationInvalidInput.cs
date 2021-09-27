using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class ComputeStandardDeviationInvalidInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, $"{Messages.NullEmptyCollection} (Parameter 'readings')" };
            yield return new object[] { new List<DecimalReadingModel>(), $"{Messages.NullEmptyCollection} (Parameter 'readings')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
