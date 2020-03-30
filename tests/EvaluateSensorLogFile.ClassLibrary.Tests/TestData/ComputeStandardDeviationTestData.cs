using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class ComputeStandardDeviationTestData : IEnumerable<object[]>
    {
        private static readonly List<ReadingModel<decimal>> readings = new List<ReadingModel<decimal>>
        {
            new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 44.4m },
            new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 43.9m },
            new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 44.9m },
            new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:07"), Value = 43.8m },
            new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:08"), Value = 42.1m }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { readings, 0.9m };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}