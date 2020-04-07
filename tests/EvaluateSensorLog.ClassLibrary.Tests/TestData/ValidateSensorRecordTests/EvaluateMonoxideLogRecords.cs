using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.ClassLibrary.Models;

namespace EvaluateSensorLog.ClassLibrary.Tests.TestData.ValidateSensorRecordTestsData
{
    public class EvaluateMonoxideLogRecords : IEnumerable<object[]>
    {
        private static readonly List<KeyValuePair<string, string>> expected = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Test-5", "keep"),
            new KeyValuePair<string, string>("Test-6", "discard")
        };

        private static readonly List<MonoxideModel> monoxideModels = new List<MonoxideModel>
        {
            new MonoxideModel
            {
                Name = "Test-5",
                Readings = new List<IntegerReadingModel>
                {
                    new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1 },
                    new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 4 },
                    new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 7 }
                }
            },
            new MonoxideModel
            {
                Name = "Test-6",
                Readings = new List<IntegerReadingModel>
                {
                    new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1 },
                    new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 4 },
                    new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 8 }
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { monoxideModels, 4, expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}