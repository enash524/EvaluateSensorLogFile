using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class HandleIntegerReadingTestData : IEnumerable<object[]>
    {
        private static readonly ReadingModel<int> expected = new ReadingModel<int>
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