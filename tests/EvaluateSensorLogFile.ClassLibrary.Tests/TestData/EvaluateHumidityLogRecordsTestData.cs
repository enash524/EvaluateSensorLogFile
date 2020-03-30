using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class EvaluateHumidityLogRecordsTestData : IEnumerable<object[]>
    {
        private static readonly List<KeyValuePair<string, string>> expected = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Test-3", "keep"),
            new KeyValuePair<string, string>("Test-4", "discard")
        };

        private static readonly List<HumidityModel> humidityModels = new List<HumidityModel>
        {
            new HumidityModel
            {
                Name = "Test-3",
                Readings = new List<ReadingModel<decimal>>
                {
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1.0m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 1.5m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 0.5m }
                }
            },
            new HumidityModel
            {
                Name = "Test-4",
                Readings = new List<ReadingModel<decimal>>
                {
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1.0m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 3.5m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 0.5m }
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { humidityModels, 1.5m, expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}