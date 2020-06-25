using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ParseSensorRecordFileTests
{
    public class HandleIntegerReading : IEnumerable<object[]>
    {
        private static readonly IntegerReadingModel expected = new IntegerReadingModel
        {
            Timestamp = DateTime.Parse("2007-04-05T22:04"),
            Value = 5
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "2007-04-05T22:04", "5", expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}