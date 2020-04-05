using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData
{
    public class HandleDecimalReadingTestData : IEnumerable<object[]>
    {
        private static readonly DecimalReadingModel expected = new DecimalReadingModel
        {
            Timestamp = DateTime.Parse("2007-04-05T22:00"),
            Value = 72.4m
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "2007-04-05T22:00", "72.4", expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}