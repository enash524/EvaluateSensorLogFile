using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLog.Domain.Models;

namespace EvaluateSensorLog.Data.Tests.TestData.ValidateSensorRecordTests
{
    public class ValidateSensorLogRecords : IEnumerable<object[]>
    {
        private static readonly ValidateSensorLogModel _expected = new ValidateSensorLogModel
        {
            HumidityResults = new List<HumidityResultModel>
            {
                new HumidityResultModel { SensorName = "Test-3", HumidityStatus = HumidityStatus.Keep },
                new HumidityResultModel { SensorName = "Test-4", HumidityStatus = HumidityStatus.Discard }
            },
            MonoxideResults = new List<MonoxideResultModel>
            {
                new MonoxideResultModel { SensorName = "Test-5", MonoxideStatus = MonoxideStatus.Keep },
                new MonoxideResultModel { SensorName = "Test-6", MonoxideStatus = MonoxideStatus.Discard }
            },
            ThermometerResults = new List<ThermometerResultModel>
            {
                new ThermometerResultModel { SensorName = "Test-1", ThermometerStatus = ThermometerStatus.UltraPrecise },
                new ThermometerResultModel { SensorName = "Test-2", ThermometerStatus = ThermometerStatus.Precise }
            }
        };

        private static readonly SensorLogModel _sensorLogModel = new SensorLogModel
        {
            HumidityReadings = new List<HumidityModel>
            {
                new HumidityModel
                {
                    Name = "hum-1",
                    Readings = new List<DecimalReadingModel>
                    {
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1.0m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 1.5m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 0.5m }
                    }
                },
                new HumidityModel
                {
                    Name = "hum-2",
                    Readings = new List<DecimalReadingModel>
                    {
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1.0m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 3.5m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 0.5m }
                    }
                }
            },
            MonoxideReadings = new List<MonoxideModel>
            {
                new MonoxideModel
                {
                    Name = "mon-1",
                    Readings = new List<IntegerReadingModel>
                    {
                        new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1 },
                        new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 4 },
                        new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 7 }
                    }
                },
                new MonoxideModel
                {
                    Name = "mon-2",
                    Readings = new List<IntegerReadingModel>
                    {
                        new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1 },
                        new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 4 },
                        new IntegerReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 8 }
                    }
                }
            },
            ReferenceValues = new ReferenceValuesModel
            {
                ThermometerReferenceValue = 70.0m,
                HumidityReferenceValue = 1.5m,
                MonoxideReferenceValue = 4
            },
            ThermometerReadings = new List<ThermometerModel>
            {
                new ThermometerModel
                {
                    Name = "temp-1",
                    Readings = new List<DecimalReadingModel>
                    {
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 70.0m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:01"), Value = 70.5m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:02"), Value = 69.5m },
                    }
                },
                new ThermometerModel
                {
                    Name = "temp-2",
                    Readings = new List<DecimalReadingModel>
                    {
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 72.4m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:01"), Value = 76.0m },
                        new DecimalReadingModel { Timestamp = DateTime.Parse("2007-04-05T22:02"), Value = 79.1m },
                    }
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { _sensorLogModel, _expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}