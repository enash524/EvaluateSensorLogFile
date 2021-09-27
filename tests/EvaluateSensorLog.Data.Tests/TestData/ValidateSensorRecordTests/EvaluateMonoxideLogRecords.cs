using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class EvaluateMonoxideLogRecords : IEnumerable<object[]>
    {
        private static readonly List<MonoxideResultModel> expected = new List<MonoxideResultModel>
        {
            new MonoxideResultModel
            {
                SensorName = "Test-5",
                MonoxideStatus = MonoxideStatus.Keep
            },
            new MonoxideResultModel
            {
                SensorName = "Test-6",
                MonoxideStatus = MonoxideStatus.Discard
            }
        };

        private static readonly List<MonoxideModel> monoxideModels = new List<MonoxideModel>
        {
            new MonoxideModel
            {
                Name = "Test-5",
                Readings = new List<IntegerReadingModel>
                {
                    new IntegerReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:04"),
                        Value = 1
                    },
                    new IntegerReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:05"),
                        Value = 4
                    },
                    new IntegerReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:06"),
                        Value = 7
                    }
                }
            },
            new MonoxideModel
            {
                Name = "Test-6",
                Readings = new List<IntegerReadingModel>
                {
                    new IntegerReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:04"),
                        Value = 1
                    },
                    new IntegerReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:05"),
                        Value = 4
                    },
                    new IntegerReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:06"),
                        Value = 8
                    }
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
