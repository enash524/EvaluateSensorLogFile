using System;
using System.Collections;
using System.Collections.Generic;
using EvaluateSensorLogFile.ClassLibrary.Models;

namespace EvaluateSensorLogFile.ClassLibrary.Tests.TestData
{
    public class EvaluateSensorLogRecordsTestData : IEnumerable<object[]>
    {
        private const string expected = "{\r\n  \"test-1\": \"ultra precise\",\r\n  \"test-2\": \"precise\",\r\n  \"test-3\": \"keep\",\r\n  \"test-4\": \"discard\",\r\n  \"test-5\": \"keep\",\r\n  \"test-6\": \"discard\"\r\n}";

        private static readonly SensorLogModel sensorLogModel = new SensorLogModel();

        public EvaluateSensorLogRecordsTestData()
        {
            ReferenceValuesModel referenceValuesModel = new ReferenceValuesModel
            {
                ThermometerReferenceValue = 70.0m,
                HumidityReferenceValue = 1.5m,
                MonoxideReferenceValue = 4
            };

            List<ThermometerModel> thermometerModels = new List<ThermometerModel>
            {
                new ThermometerModel
                {
                    Name = "Test-1",
                    Readings = new List<ReadingModel<decimal>>
                    {
                        new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 70.0m },
                        new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:01"), Value = 70.5m },
                        new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:02"), Value = 69.5m },
                    }
                },
                new ThermometerModel
                {
                    Name = "Test-2",
                    Readings = new List<ReadingModel<decimal>>
                    {
                        new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:00"), Value = 72.4m },
                        new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:01"), Value = 76.0m },
                        new ReadingModel<decimal> { Timestamp = DateTime.Parse("2007-04-05T22:02"), Value = 79.1m },
                    }
                }
            };

            List<HumidityModel> humidityModels = new List<HumidityModel>
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

            List<MonoxideModel> monoxideModels = new List<MonoxideModel>
            {
                new MonoxideModel
                {
                    Name = "Test-5",
                    Readings = new List<ReadingModel<int>>
                    {
                        new ReadingModel<int> { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1 },
                        new ReadingModel<int> { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 4 },
                        new ReadingModel<int> { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 7 }
                    }
                },
                new MonoxideModel
                {
                    Name = "Test-6",
                    Readings = new List<ReadingModel<int>>
                    {
                        new ReadingModel<int> { Timestamp = DateTime.Parse("2007-04-05T22:04"), Value = 1 },
                        new ReadingModel<int> { Timestamp = DateTime.Parse("2007-04-05T22:05"), Value = 4 },
                        new ReadingModel<int> { Timestamp = DateTime.Parse("2007-04-05T22:06"), Value = 8 }
                    }
                }
            };

            sensorLogModel.ReferenceValues = referenceValuesModel;
            sensorLogModel.ThermometerReadings = thermometerModels;
            sensorLogModel.HumidityReadings = humidityModels;
            sensorLogModel.MonoxideReadings = monoxideModels;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { sensorLogModel, expected };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}