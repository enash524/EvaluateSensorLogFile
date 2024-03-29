﻿using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class EvaluateThermometerLogRecords : IEnumerable<object[]>
    {
        private static readonly List<ThermometerResultModel> expected = new List<ThermometerResultModel>
        {
            new ThermometerResultModel
            {
                SensorName = "Test-1",
                ThermometerStatus = ThermometerStatus.UltraPrecise
            },
            new ThermometerResultModel
            {
                SensorName = "Test-2",
                ThermometerStatus = ThermometerStatus.Precise
            }
        };

        private static readonly List<ThermometerModel> thermometerModels = new List<ThermometerModel>
        {
            new ThermometerModel
            {
                Name = "Test-1",
                Readings = new List<DecimalReadingModel>
                {
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:00"),
                        Value = 70.0m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:01"),
                        Value = 70.5m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:02"),
                        Value = 69.5m
                    }
                }
            },
            new ThermometerModel
            {
                Name = "Test-2",
                Readings = new List<DecimalReadingModel>
                {
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:00"),
                        Value = 62.4m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:01"),
                        Value = 76.0m
                    },
                    new DecimalReadingModel
                    {
                        Timestamp = DateTime.Parse("2007-04-05T22:02"),
                        Value = 89.1m
                    }
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
