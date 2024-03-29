﻿using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class EvaluateHumidityLogRecords : IEnumerable<object[]>
    {
        private static readonly List<HumidityResultModel> expected = new List<HumidityResultModel>
        {
            new HumidityResultModel
            {
                SensorName = "Test-3",
                HumidityStatus = HumidityStatus.Keep
            },
            new HumidityResultModel
            {
                SensorName = "Test-4",
                HumidityStatus = HumidityStatus.Discard
            }
        };

        private static readonly List<HumidityModel> humidityModels = new List<HumidityModel>
        {
            new HumidityModel
            {
                Name = "Test-3",
                Readings = new List<DecimalReadingModel>
                {
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:04"),
                        Value = 1.0m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:05"),
                        Value = 1.5m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:06"),
                        Value = 0.5m
                    }
                }
            },
            new HumidityModel
            {
                Name = "Test-4",
                Readings = new List<DecimalReadingModel>
                {
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:04"),
                        Value = 1.0m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:05"),
                        Value = 3.5m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:06"),
                        Value = 0.5m
                    }
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
