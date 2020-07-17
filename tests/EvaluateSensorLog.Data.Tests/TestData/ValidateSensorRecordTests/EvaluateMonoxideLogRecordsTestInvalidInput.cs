using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class EvaluateMonoxideLogRecordsTestInvalidInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, 0, $"{Messages.NullEmptyCollection} (Parameter 'monoxideModel')" };
            yield return new object[] { new List<MonoxideModel>(), 0, $"{Messages.NullEmptyCollection} (Parameter 'monoxideModel')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}