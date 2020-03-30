﻿using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class EvaluateThermometerLogRecordsTestData : IEnumerable<object[]>
    {
        private static readonly List<KeyValuePair<string, string>> expected = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Test-1", "ultra precise"),
            new KeyValuePair<string, string>("Test-2", "precise")
        };

        private static readonly List<ThermometerModel> thermometerModels = new List<ThermometerModel>
        {
            new ThermometerModel
            {
                Name = "Test-1",
                Readings = new List<ReadingModel<decimal>>
                {
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 70.0m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:01"), Value = 70.5m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:02"), Value = 69.5m }
                }
            },
            new ThermometerModel
            {
                Name = "Test-2",
                Readings = new List<ReadingModel<decimal>>
                {
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 62.4m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:01"), Value = 76.0m },
                    new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:02"), Value = 89.1m }
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { thermometerModels, 70.0m, expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}