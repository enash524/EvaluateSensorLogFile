using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class EvaluateThermometerLogRecordsInvalidInput : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, 0, $"{Messages.NullEmptyCollection} (Parameter 'thermometerModel')" };
            yield return new object[] { new List<ThermometerModel>(), 0, $"{Messages.NullEmptyCollection} (Parameter 'thermometerModel')" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
