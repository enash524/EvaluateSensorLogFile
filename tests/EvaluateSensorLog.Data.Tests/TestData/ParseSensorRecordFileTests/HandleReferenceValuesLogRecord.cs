using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ParseSensorRecordFileTests
{
    public class HandleReferenceValuesLogRecord : IEnumerable<object[]>
    {
        private static readonly ReferenceValuesModel expected = new ReferenceValuesModel
        {
            ThermometerReferenceValue = 70.0m,
            HumidityReferenceValue = 45.0m,
            MonoxideReferenceValue = 6
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "70.0", "45.0", "6", expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
